using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using STJDB;
using System.Threading;
using System.Collections.Generic;

public class 服务器 : MonoBehaviour
{
    private TcpListener 监听器;
    private bool 运行中 = true;

    TcpClient 主户端;
    TcpClient 客户端;
    NetworkStream 主流;
    NetworkStream 客流;
    bool 是否主创 = true;

    Thread 主户端监听线程;
    Thread 客户端监听线程;

    void Start()
    {
        // 启动服务器
        IPAddress ip = IPAddress.Parse("0.0.0.0");
        try
        {
            监听器 = new TcpListener(ip, 8848);
            监听器.Start();
        }
        catch (SocketException ex)
        {
            Debug.Log($"端口被占用，服务器未启动: {ex.Message}");
            运行中 = false;
            return;
        }
        Debug.Log("服务器已启动，等待连接...");
        StartCoroutine(接受主客户端连接());
    }

    private IEnumerator 接受主客户端连接()
    {
        int a = 0;
        while (运行中)
        {
            if (监听器.Pending())
            {
                if (a >= 2) StopCoroutine(nameof(接受主客户端连接));
                if (是否主创) 
                {
                    主户端 = 监听器.AcceptTcpClient();
                    Debug.Log($"主户端已连接: {主户端.Client.RemoteEndPoint}");
                    主流 = 主户端.GetStream();
                    主户端监听线程 = new Thread(主户端监听);
                    主户端监听线程.IsBackground = true;
                    主户端监听线程.Start();
                    是否主创 = false;
                }
                else
                {
                    客户端 = 监听器.AcceptTcpClient();
                    Debug.Log($"客户端已连接: {客户端.Client.RemoteEndPoint}");
                    客流 = 客户端.GetStream();
                    客户端监听线程 = new Thread(客户端监听);
                    客户端监听线程.IsBackground = true;
                    客户端监听线程.Start();
                }
                a++;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 发送消息时加4字节长度头
    void 消息发送(byte[] 数据, NetworkStream 流) 
    {
        if(流 == null) 
        {
            Debug.Log("目标流还未连接");
            return;
        }
        try
        {
            int len = 数据.Length;
            byte[] lenBytes = System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(len));
            流.Write(lenBytes, 0, 4);
            流.Write(数据, 0, len);
            流.Flush();
            //Debug.Log($"消息转发成功，长度: {数据.Length}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"消息发送失败: {ex.Message}");
        }
    }

    private void 主户端监听()
    {
        List<byte> 主缓冲区 = new();
        byte[] 临时 = new byte[1024];
        聊天消息 消息 = new()
        {
            消息 = "执子者切换"
        };
        byte[] 指令 = 协议工具类.打包<聊天消息>(消息类型.聊天消息, 消息);
        消息发送(指令, 主流);
        while (主户端.Connected)
        {
            try
            {
                int 字节数 = 主流.Read(临时, 0, 临时.Length);
                if (字节数 > 0)
                {
                    主缓冲区.AddRange(new System.ArraySegment<byte>(临时, 0, 字节数));
                    // 分包处理
                    while (主缓冲区.Count >= 4)
                    {
                        int msgLen = System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt32(主缓冲区.ToArray(), 0));
                        if (主缓冲区.Count >= 4 + msgLen)
                        {
                            byte[] msgBytes = 主缓冲区.GetRange(4, msgLen).ToArray();
                            主缓冲区.RemoveRange(0, 4 + msgLen);
                            Debug.Log($"主户端发送完整消息，长度: {msgLen}");
                            // 用主线程调度器转发
                            主线程调度器.唯一单例.任务入队(() => 消息发送(msgBytes, 客流));
                        }
                        else
                        {
                            break; // 不够一条完整消息，等下次Read
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"处理主户端通信时出错: {ex.Message}");
                break;
            }
        }
        Debug.Log("主户端监听线程结束");
    }

    private void 客户端监听()
    {
        List<byte> 客缓冲区 = new();
        byte[] 临时 = new byte[1024];
        while (客户端.Connected)
        {
            try
            {
                int 字节数 = 客流.Read(临时, 0, 临时.Length);
                if (字节数 > 0)
                {
                    客缓冲区.AddRange(new System.ArraySegment<byte>(临时, 0, 字节数));
                    // 分包处理
                    while (客缓冲区.Count >= 4)
                    {
                        int msgLen = System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt32(客缓冲区.ToArray(), 0));
                        if (客缓冲区.Count >= 4 + msgLen)
                        {
                            byte[] msgBytes = 客缓冲区.GetRange(4, msgLen).ToArray();
                            客缓冲区.RemoveRange(0, 4 + msgLen);
                            Debug.Log($"客户端发送完整消息，长度: {msgLen}");
                            // 用主线程调度器转发
                            主线程调度器.唯一单例.任务入队(() => 消息发送(msgBytes, 主流));
                        }
                        else
                        {
                            break; // 不够一条完整消息，等下次Read
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"处理客户端通信时出错: {ex.Message}");
                break;
            }
        }
        Debug.Log("客户端监听线程结束");
    }
}
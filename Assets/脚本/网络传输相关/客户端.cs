using STJDB;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class 客户端
{
    public static TcpClient 玩家;
    public static NetworkStream 玩家流;
    private static 客户端监听器 监听器实例;

    public static bool 连接服务器(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip) || !IPAddress.TryParse(ip, out var address))
        {
            Debug.Log("无效的IP地址！");
            return false;
        }
        玩家 = new TcpClient();
        玩家.Connect(address, 8848);
        玩家流 = 玩家.GetStream();
        Debug.Log("连接服务器成功！");
        开始监听();
        return true;
    }

    public static event System.Action<byte[]> 收到消息;

    // 发送消息时加4字节长度头
    public static void 发送消息<T>(消息类型 类型, T 消息)
    {
        if (玩家流 == null || !玩家流.CanWrite) 
        {
            //Debug.LogWarning("网络流不可用，无法发送消息");
            return;
        }
        try
        {
            //Debug.Log($"准备发送消息，类型: {类型}");
            byte[] 数据 = 协议工具类.打包<T>(类型, 消息);
            int len = 数据.Length;
            byte[] lenBytes = System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(len));
            玩家流.Write(lenBytes, 0, 4);
            玩家流.Write(数据, 0, len);
            玩家流.Flush();
            Debug.Log($"消息发送成功，长度: {数据.Length}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"发送消息失败: {ex.Message}");
        }
    }

    public static void 开始监听()
    {
        if (玩家 == null) return;
        if (监听器实例 == null)
        {
            var go = new GameObject("客户端监听器");
            Object.DontDestroyOnLoad(go);
            监听器实例 = go.AddComponent<客户端监听器>();
        }
        监听器实例.Start监听();
    }

    // 添加一个方法来触发事件
    public static void 触发收到消息(byte[] 数据)
    {
        收到消息?.Invoke(数据);
    }
}

public class 客户端监听器 : MonoBehaviour
{
    private Coroutine 协程实例;
    public void Start监听()
    {
        if (协程实例 != null)
        {
            StopCoroutine(协程实例);
        }
        协程实例 = StartCoroutine(监听协程());
    }

    private IEnumerator 监听协程()
    {
        List<byte> 缓冲区 = new List<byte>();
        byte[] 临时 = new byte[1024];
        while (客户端.玩家 != null && 客户端.玩家.Connected)
        {
            if (客户端.玩家流 != null && 客户端.玩家流.DataAvailable)
            {
                int 字节数 = 0;
                try
                {
                    字节数 = 客户端.玩家流.Read(临时, 0, 临时.Length);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"监听协程读取出错: {ex.Message}");
                    break;
                }
                if (字节数 > 0)
                {
                    缓冲区.AddRange(new System.ArraySegment<byte>(临时, 0, 字节数));
                    // 分包处理
                    while (缓冲区.Count >= 4)
                    {
                        int msgLen = System.Net.IPAddress.NetworkToHostOrder(System.BitConverter.ToInt32(缓冲区.ToArray(), 0));
                        if (缓冲区.Count >= 4 + msgLen)
                        {
                            byte[] msgBytes = 缓冲区.GetRange(4, msgLen).ToArray();
                            缓冲区.RemoveRange(0, 4 + msgLen);
                            //Debug.Log($"收到完整消息（协程），长度: {msgLen}");
                            主线程调度器.唯一单例.任务入队(() => 客户端.触发收到消息(msgBytes));
                        }
                        else
                        {
                            break; // 不够一条完整消息，等下次Read
                        }
                    }
                }
            }
            yield return null; // 每帧检查一次
        }
        Debug.Log("客户端监听协程结束");
    }
}

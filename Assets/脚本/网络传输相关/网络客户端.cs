using System.Net.Sockets;
using System.Net;
using UnityEngine;
using STJDB协议类库;

public static class 网络客户端
{
    public static TcpClient 客户端;
    public static NetworkStream 网络流;
    private static string 最后连接的IP;

    public static void 连接服务器(string ip)
    {
        最后连接的IP = ip;
        客户端 = new TcpClient();
        客户端.Connect(IPAddress.Parse(ip), 8848);
        网络流 = 客户端.GetStream();
        Debug.Log("连接服务器成功！");
    }

    public static event System.Action<object> 收到消息;

    public static void 发送消息<T>(T 消息)
    {
        if (网络流 == null || !网络流.CanWrite) return;

        byte[] 数据 = 网络工具类.序列化(消息);
        网络流.Write(数据, 0, 数据.Length);
    }

    public static void 开始监听()
    {
        if (客户端 == null) return;
        new System.Threading.Thread(() =>
        {
            byte[] 缓冲区 = new byte[1024];
            while (客户端.Connected)
            {
                try
                {
                    int 字节数 = 网络流.Read(缓冲区, 0, 缓冲区.Length);
                    if (字节数 > 0)
                    {
                        byte[] 数据 = new byte[字节数];
                        System.Array.Copy(缓冲区, 数据, 字节数);
                        object 消息 = 网络工具类.反序列化<object>(数据);
                        // 通过主线程派发消息
                        主线程调度器.唯一单例.任务入队(() => 收到消息?.Invoke(消息));
                    }
                }
                catch { }
            }
        }).Start();
    }
}

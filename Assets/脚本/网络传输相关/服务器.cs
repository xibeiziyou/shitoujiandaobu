using STJDB协议类库;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class 服务器 : MonoBehaviour
{
    private TcpListener 监听器;
    private bool 运行中 = true;

    void Start()
    {
        // 启动服务器
        IPAddress ip = IPAddress.Parse("0.0.0.0");
        监听器 = new TcpListener(ip, 8848);
        监听器.Start();
        Debug.Log("服务器已启动，等待连接...");
        StartCoroutine(接受客户端连接());
    }

    private IEnumerator 接受客户端连接()
    {
        while (运行中)
        {
            if (监听器.Pending())
            {
                TcpClient 客户端 = 监听器.AcceptTcpClient();
                Debug.Log($"客户端已连接: {客户端.Client.RemoteEndPoint}");
                StartCoroutine(处理客户端通信(客户端));
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 修复4：统一命名并添加参数
    private IEnumerator 处理客户端通信(TcpClient 客户端)
    {
        NetworkStream 流 = 客户端.GetStream();
        byte[] 缓冲区 = new byte[1024];

        while (客户端.Connected)
        {
            if (流.DataAvailable)
            {
                int 字节数 = 流.Read(缓冲区, 0, 缓冲区.Length);
                byte[] 数据 = new byte[字节数];
                System.Array.Copy(缓冲区, 数据, 字节数);

                object 消息 = 网络工具类.反序列化<object>(数据);

                if (消息 is 注册 注册消息)
                {
                    处理注册(注册消息, 流);
                }
                else if (消息 is 下棋操作 下棋消息)
                {
                    处理下棋操作(下棋消息, 流);
                }
            }
            yield return null;
        }
    }

    // 其他处理方法和游戏逻辑...

    void 处理注册(注册 消息, NetworkStream 流)
    {
        int 房间号 = 房间管理.创建房间(流);
        消息.是否成功 = true;

        byte[] 回复数据 = 网络工具类.序列化(消息);
        流.Write(回复数据, 0, 回复数据.Length);
    }

    void 处理下棋操作(下棋操作 消息, NetworkStream 流)
    {
        if (!房间管理.所有房间.TryGetValue(消息.房间号码, out 房间 房间))
        {
            消息.操作结果 = false;
            return;
        }

        if (消息.X < -1.5 || 消息.X > 1.5 || 消息.Y < -1.5 || 消息.Y > 1.5)
        {
            消息.操作结果 = false;
            return;
        }

        // 更新棋盘
        房间.棋盘[消息.X, 消息.Y] = 消息.棋子类型;
        消息.操作结果 = true;

        // 广播给所有玩家
        foreach (var 玩家流 in 房间.玩家列表)
        {
            byte[] 数据 = 网络工具类.序列化(消息);
            玩家流.Write(数据, 0, 数据.Length);
        }
    }
}
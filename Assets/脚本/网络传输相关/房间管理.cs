using STJDB协议类库;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class 房间
{
    public int 房间号;
    public List<NetworkStream> 玩家列表 = new List<NetworkStream>();
    public 棋子类型[,] 棋盘 = new 棋子类型[3, 3];
}

public static class 房间管理
{
    public static Dictionary<int, 房间> 所有房间 = new Dictionary<int, 房间>();
    private static int 当前最大房间号 = 1;

    public static int 创建房间(NetworkStream 玩家)
    {
        var 新房间 = new 房间
        {
            房间号 = 当前最大房间号++,
            棋盘 = new 棋子类型[3, 3]
        };
        所有房间.Add(新房间.房间号, 新房间);
        return 新房间.房间号;
    }

    public static bool 加入房间(int 房间号, NetworkStream 玩家)
    {
        if (!所有房间.ContainsKey(房间号)) return false;

        var 房间 = 所有房间[房间号];
        if (房间.玩家列表.Count >= 2) return false;

        房间.玩家列表.Add(玩家);
        return true;
    }
}
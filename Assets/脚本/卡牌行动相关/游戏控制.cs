using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 游戏控制 : 单例类<游戏控制>
{
    public enum 执子者
    {
        主方,
        客方
    }

    public 执子者 当前执子者 = 执子者.主方;

    public void 执子者切换() 
    {
        当前执子者 = (执子者)(((int)当前执子者 + 1) % 2);
    }

    public bool 执子判断(int 所执子) 
    {
        return 所执子 == (int)当前执子者;
    }
}

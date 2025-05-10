using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class 敌牌 : 棋牌基类
{
    public new void Start()
    {
        base.Start();

        主客方 = 1;
    }
}

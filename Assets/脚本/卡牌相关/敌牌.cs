using DG.Tweening;
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

    public override void OnMouseDown()
    {
        if (执子控制.唯一单例.是否联机 == true) return;
        base.OnMouseDown();
    }

    public override void OnMouseUp()
    {
        if (执子控制.唯一单例.是否联机 == true) return;
        base.OnMouseUp();
    }

    public void 卡牌转换() 
    {
        激活数 = (激活数 + 1) % 2;
        激活牌型 = 自身牌型[激活数];
        transform.DORotate(new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            180 * 激活数), 1);
        牌型调整();
    }
}

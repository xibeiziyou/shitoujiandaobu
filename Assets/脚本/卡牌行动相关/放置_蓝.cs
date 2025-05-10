using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class 放置_蓝 : MonoBehaviour
{
    Vector3 合并位置;

    public 棋牌基类 目标牌;

    private void OnMouseDown()
    {
        棋牌基类.当前放大牌.可行动位隐藏();
        目标牌.transform.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        合并位置 = (棋牌基类.当前放大牌.transform.position + 目标牌.transform.position) / 2;
        棋牌基类.当前放大牌.transform.DOMove(合并位置, 1);
        目标牌.transform.DOMove(合并位置, 1).OnKill(() =>
        {
            回到牌组(棋牌基类.当前放大牌.GetComponent<棋牌基类>());
            回到牌组(目标牌);
            棋牌基类.当前放大牌 = null; Debug.Log("当前放大牌变成空");
            游戏控制.唯一单例.执子者切换();
        });
    }
    void 回到牌组(棋牌基类 牌) 
    {
        if (牌.主客方 == 0) 
        {
            主手牌管理.唯一单例.手牌加入(牌.gameObject);
        }
        else 
        {
            客手牌管理.唯一单例.手牌加入(牌.gameObject);
        }
    }

    public void 设置目标牌( 棋牌基类 牌) 
    {
        目标牌 = 牌;
        Debug.Log(目标牌);
    }
}

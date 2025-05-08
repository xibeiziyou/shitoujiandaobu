using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 放置提示 : MonoBehaviour
{

    private void OnMouseDown()
    {
        if (棋牌基类.当前放大牌.手牌)
        {
            手牌管理.唯一单例.手牌退出();
            棋牌基类.当前放大牌.transform.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack);
            Debug.Log("?");
        }
        棋牌基类.当前放大牌.transform.position = this.transform.position;
        棋牌基类.当前放大牌.恢复原状(() => { 棋牌基类.当前放大牌.可行动位隐藏(); 棋牌基类.当前放大牌 = null; });
    }

}

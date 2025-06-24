using DG.Tweening;
using UnityEngine;

public class 放置_红 : MonoBehaviour
{

    棋牌基类 目标牌;

    Vector3 目标位置;
    private void OnMouseDown()
    {
        if (棋牌基类.当前放大牌.手牌)
        {
            棋牌基类.当前放大牌.可行动位隐藏();
            棋牌基类.当前放大牌.恢复原状();
            游戏控制.唯一单例.执子者切换();
            return;
        }
        棋牌基类.当前放大牌.可行动位隐藏();
        目标牌.transform.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        目标牌.transform.position = 目标位置;
        目标牌.恢复原状();
        回到牌组(棋牌基类.当前放大牌.GetComponent<棋牌基类>());
        棋牌基类.当前放大牌 = null; //Debug.Log("当前放大牌变成空");
        游戏控制.唯一单例.执子者切换();
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

    public void 设置目标牌(棋牌基类 牌)
    {
        目标牌 = 牌;

        目标位置 = 牌.transform.position;
    }
}

using DG.Tweening;
using UnityEngine;

public class 放置_绿 : MonoBehaviour
{

    棋牌基类 目标牌;

    Vector3 目标位置;
    private void OnMouseDown()
    {
        if (棋牌基类.当前放大牌.手牌)
        {
            if (游戏控制.唯一单例.当前执子者 == 游戏控制.执子者.主方) 主手牌管理.唯一单例.手牌退出();
            else { 客手牌管理.唯一单例.手牌退出(); }
            棋牌基类.当前放大牌.可行动位隐藏();
            棋牌基类.当前放大牌.transform.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack);
            棋牌基类.当前放大牌.transform.position = 目标位置;
            回到牌组(目标牌);
            棋牌基类.当前放大牌 = null; //Debug.Log("当前放大牌变成空");
            游戏控制.唯一单例.执子者切换();
            return;
        }
        棋牌基类.当前放大牌.可行动位隐藏();
        目标牌.transform.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        棋牌基类.当前放大牌.transform.position = 目标位置;
        棋牌基类.当前放大牌.恢复原状();
        回到牌组(目标牌);
        棋牌基类.当前放大牌 = null;// Debug.Log("当前放大牌变成空");
        游戏控制.唯一单例.执子者切换();
    }

    void 回到牌组(棋牌基类 牌)
    {
        if (是否为王(牌)) 
        {
            if (牌.主客方 == 0) 
            {
                主命数.命数--;
                if(主命数.命数显示) 主命数.命数显示.text = 主命数.命数.ToString();
                if (主命数.命数 == 0) 
                {
                    棋牌基类.行动点数 = 0;
                    Debug.Log("客胜");
                }
            }
            else 
            {
                客命数.命数--;
                if (客命数.命数显示) 客命数.命数显示.text = 客命数.命数.ToString();
                if (客命数.命数 == 0)
                {
                    棋牌基类.行动点数 = 0;
                    Debug.Log("主胜");
                }
            }
            对象池.唯一单例.加入对象(牌.gameObject);
            return;
        }
        if (牌.主客方 == 0)
        {
            主手牌管理.唯一单例.手牌加入(牌.gameObject);
        }
        else
        {
            客手牌管理.唯一单例.手牌加入(牌.gameObject);
        }
    }

    bool 是否为王(棋牌基类 牌) 
    {
        foreach (var item in 牌.自身牌型)
        {
            if (item == 棋牌基类.牌型.王) return true;
        }
        return false;
    }

    public void 设置目标牌(棋牌基类 牌)
    {
        目标牌 = 牌;

        目标位置 = 牌.transform.position;
    }
}

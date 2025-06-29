using DG.Tweening;
using UnityEngine;
using STJDB;

public class 放置_绿 : MonoBehaviour
{
    棋牌基类 目标牌;
    private readonly 吃牌消息 吃牌 = new();
    UnityEngine.Vector3 目标位置; // 修改为 UnityEngine.Vector3

    private void OnMouseDown()
    {
        音频管理器.唯一单例.音效播放("放下", false);
        if (棋牌基类.当前放大牌.手牌)
        {
            if (执子控制.唯一单例.当前执子者 == 执子控制.执子者.主方) 主手牌管理.唯一单例.手牌退出();
            else { 客手牌管理.唯一单例.手牌退出(); }
            棋牌基类.当前放大牌.可行动位隐藏();
            棋牌基类.当前放大牌.transform.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack);
            棋牌基类.当前放大牌.transform.position = 目标位置;
            if (执子控制.唯一单例.是否联机)
            {
                吃牌.牌名 = 棋牌基类.当前放大牌.name;
                吃牌.被吃牌名 = 目标牌.name;
                吃牌.被吃位置 = new System.Numerics.Vector3(目标位置.x, 目标位置.y, 目标位置.z); // 显式转换为 System.Numerics.Vector3
                客户端.发送消息<吃牌消息>(消息类型.吃牌消息, 吃牌);
                if (是否为王(目标牌))
                {
                    Destroy(目标牌.gameObject);
                }
                else
                {
                    目标牌.transform.position = new Vector3(0, 0, 10);
                }
            }
            else 
            {
                回到牌组(目标牌);
            }
            棋牌基类.当前放大牌 = null; //Debug.Log("当前放大牌变成空");
            执子控制.唯一单例.执子者切换();
            return;
        }
        棋牌基类.当前放大牌.可行动位隐藏();
        目标牌.transform.DOScale(UnityEngine.Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        棋牌基类.当前放大牌.transform.position = 目标位置;
        if (执子控制.唯一单例.是否联机)
        {
            吃牌.牌名 = 棋牌基类.当前放大牌.name;
            吃牌.被吃牌名 = 目标牌.name;
            吃牌.被吃位置 = new System.Numerics.Vector3(目标位置.x, 目标位置.y, 目标位置.z); // 显式转换为 System.Numerics.Vector3
            客户端.发送消息<吃牌消息>(消息类型.吃牌消息, 吃牌);
            if (是否为王(目标牌)) 
            {
                Destroy(目标牌.gameObject);
            }
            else 
            {
                目标牌.transform.position = new Vector3(0, 0, 10);
            }
        }
        else
        {
            回到牌组(目标牌);
        }
        棋牌基类.当前放大牌.恢复原状();
        棋牌基类.当前放大牌 = null;// Debug.Log("当前放大牌变成空");
        执子控制.唯一单例.执子者切换();
    }

    void 回到牌组(棋牌基类 牌)
    {
        if (是否为王(牌))
        {
            if (牌.主客方 == 0)
            {
                主命数.命数--;
                if (主命数.命数显示) 主命数.命数显示.text = 主命数.命数.ToString();
                if (主命数.命数 == 0)
                {
                    棋牌基类.行动点数 = 0;
                    Debug.Log("游戏胜利！胜利者：" + 执子控制.唯一单例.当前执子者);
                    胜负图.唯一单例.胜利();
                }
            }
            else
            {
                客命数.命数--;
                if (客命数.命数显示) 客命数.命数显示.text = 客命数.命数.ToString();
                if (客命数.命数 == 0)
                {
                    棋牌基类.行动点数 = 0;
                    Debug.Log("游戏胜利！胜利者：" + 执子控制.唯一单例.当前执子者);
                    胜负图.唯一单例.胜利();
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

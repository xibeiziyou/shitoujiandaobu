using DG.Tweening;
using UnityEngine;
using STJDB;

public class 放置_红 : MonoBehaviour
{

    棋牌基类 目标牌;
    private readonly 被冲消息 被冲;
    Vector3 目标位置;
    private void OnMouseDown()
    {
        音频管理器.唯一单例.音效播放("放下", false);
        棋牌基类.当前放大牌.可行动位隐藏();
        if (执子控制.唯一单例.是否联机)
        {
            被冲.牌名 = 棋牌基类.当前放大牌.name;
            客户端.发送消息<被冲消息>(消息类型.被冲消息, 被冲);
        }
        棋牌基类.当前放大牌.恢复原状();
        回到牌组(棋牌基类.当前放大牌.GetComponent<棋牌基类>());
        棋牌基类.当前放大牌 = null; //Debug.Log("当前放大牌变成空");
        执子控制.唯一单例.执子者切换();
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

using DG.Tweening;
using STJDB;
using UnityEngine;

public class 放置_蓝 : MonoBehaviour
{

    public 棋牌基类 目标牌;
    private readonly 对冲消息 对冲 = new();
    private void OnMouseDown()
    {
        音频管理器.唯一单例.音效播放("放下", false);
        棋牌基类.当前放大牌.可行动位隐藏();
        目标牌.transform.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        回到牌组(棋牌基类.当前放大牌.GetComponent<棋牌基类>());
        if (执子控制.唯一单例.是否联机)
        {
            对冲.牌名 = 棋牌基类.当前放大牌.name;
            对冲.对冲牌名 = 目标牌.name;
            客户端.发送消息<对冲消息>(消息类型.对冲消息, 对冲);
            目标牌.transform.position = new Vector3(0, 0, 10);
        }
        else
        {
            回到牌组(目标牌);
        }
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

    public void 设置目标牌( 棋牌基类 牌) 
    {
        目标牌 = 牌;
        Debug.Log(目标牌);
    }
}

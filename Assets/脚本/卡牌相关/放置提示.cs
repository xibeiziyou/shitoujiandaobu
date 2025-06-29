using DG.Tweening;
using UnityEngine;
using STJDB;

public class 放置提示 : MonoBehaviour
{
    private readonly 移动消息 消息 = new();
    private void OnMouseDown()
    {
        音频管理器.唯一单例.音效播放("放下",false);
        if (棋牌基类.当前放大牌.手牌)
        {
            if(执子控制.唯一单例.当前执子者 == 执子控制.执子者.主方)主手牌管理.唯一单例.手牌退出();
            else { 客手牌管理.唯一单例.手牌退出(); }
            棋牌基类.当前放大牌.transform.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack);
        }
        棋牌基类.当前放大牌.transform.position = this.transform.position;
        消息.牌名 = 棋牌基类.当前放大牌.name;
        // Update the problematic line to explicitly convert UnityEngine.Vector3 to System.Numerics.Vector3
        消息.移动位置 = new System.Numerics.Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        客户端.发送消息<移动消息>(消息类型.移动消息,消息);
        if ((棋牌基类.当前放大牌.transform.position.z == 1.5 && 棋牌基类.当前放大牌.主客方 == 0) || (棋牌基类.当前放大牌.transform.position.z == -1.5 && 棋牌基类.当前放大牌.主客方 == 1))
            if (棋牌基类.当前放大牌.激活牌型 == 棋牌基类.牌型.王)
            {
                棋牌基类.行动点数 = 0;
                Debug.Log("游戏胜利！胜利者：" + 执子控制.唯一单例.当前执子者);
                胜负图.唯一单例.胜利();
            }
        棋牌基类.当前放大牌.恢复原状(() =>
        {
            棋牌基类.当前放大牌.可行动位隐藏(); 棋牌基类.当前放大牌 = null; //Debug.Log("当前放大牌变成空");
        });
        执子控制.唯一单例.执子者切换();
    }

}

using DG.Tweening;
using UnityEngine;

public class 放置提示 : MonoBehaviour
{

    private void OnMouseDown()
    {
        if (棋牌基类.当前放大牌.手牌)
        {
            if(游戏控制.唯一单例.当前执子者 == 游戏控制.执子者.主方)主手牌管理.唯一单例.手牌退出();
            else { 客手牌管理.唯一单例.手牌退出(); }
            棋牌基类.当前放大牌.transform.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack);
        }
        棋牌基类.当前放大牌.transform.position = this.transform.position;
        if ((棋牌基类.当前放大牌.transform.position.z == 1.5 && 棋牌基类.当前放大牌.主客方 == 0) || (棋牌基类.当前放大牌.transform.position.z == -1.5 && 棋牌基类.当前放大牌.主客方 == 1))
            if (棋牌基类.当前放大牌.激活牌型 == 棋牌基类.牌型.王)
            {
                棋牌基类.行动点数 = 0;
                Debug.Log("游戏胜利！胜利者：" + 游戏控制.唯一单例.当前执子者);
            }
        棋牌基类.当前放大牌.恢复原状(() =>
        {
            棋牌基类.当前放大牌.可行动位隐藏(); 棋牌基类.当前放大牌 = null; //Debug.Log("当前放大牌变成空");
        });
        游戏控制.唯一单例.执子者切换();
    }

}

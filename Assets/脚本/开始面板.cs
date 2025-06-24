
using UnityEngine.UI;

public class 开始面板 : 面板基类
{

    Button 联机模式;
    Button 自弈模式;
    Button 退出游戏;
    Button 设置;
    void Start()
    {
        联机模式 = transform.GetChild(1).GetComponent<Button>();
        自弈模式 = transform.GetChild(2).GetComponent<Button>();
        退出游戏 = transform.GetChild(3).GetComponent<Button>();
        设置 = transform.GetChild(4).GetComponent<Button>();

        联机模式.onClick.AddListener(() =>
        {

        });

        自弈模式.onClick.AddListener(() =>
        {

        });

        退出游戏.onClick.AddListener(() =>
        {

        });

        设置.onClick.AddListener(() =>
        {

        });
    }
}

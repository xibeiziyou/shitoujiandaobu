using UnityEngine;
using UnityEngine.UI;

public class 游戏面板 : 面板基类
{

    Button 设置;
    Button 返回;

    private void Start()
    {
        音频管理器.唯一单例.播放BGM("bgm");

        设置 = transform.GetChild(0).GetComponent<Button>();
        返回 = transform.GetChild(1).GetComponent<Button>();

        设置.onClick.AddListener(() => 
        {
            UI管理器.唯一单例.面板弹出(面板类型.设置面板);
        });

        返回.onClick.AddListener(() => 
        {
            返回.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("主场景");
            });
        });
    }


}

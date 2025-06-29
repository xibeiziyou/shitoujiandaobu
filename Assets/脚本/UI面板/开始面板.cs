using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class 开始面板 : 面板基类
{
    GameObject 卡牌层;
    Button 联机模式;
    Button 自弈模式;
    Button 退出游戏;
    Button 设置;

    public int 旋转卡牌数 = 10;
    public float a, b;
    List<旋转卡牌> 卡牌组;
    旋转卡牌 卡牌;
    void Start()
    {
        音频管理器.唯一单例.播放BGM("bgm");

        卡牌组 = new List<旋转卡牌> ();

        卡牌层 = transform.GetChild(0).gameObject;
        联机模式 = transform.GetChild(2).GetComponent<Button>();
        自弈模式 = transform.GetChild(3).GetComponent<Button>();
        退出游戏 = transform.GetChild(4).GetComponent<Button>();
        设置 = transform.GetChild(5).GetComponent<Button>();

        联机模式.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("联机场景");
        });

        自弈模式.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("自弈场景");
        });

        退出游戏.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        设置.onClick.AddListener(() =>
        {
            UI管理器.唯一单例.面板弹出(面板类型.设置面板);
        });

        for (int i = 0; i < 旋转卡牌数; i++)
        {
            卡牌 = 对象池.唯一单例.取出对象("旋转卡牌").GetComponent<旋转卡牌>();
            卡牌.transform.SetParent(卡牌层.transform, false);
            卡牌.transform.position = new Vector3(Random.Range(-100, 1000), -100, 0);
            卡牌组.Add(卡牌);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            音频管理器.唯一单例.音效播放("点击",false);
            Vector2 本地坐标;
            var canvas = GetComponentInParent<Canvas>();
            Camera uiCamera = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                uiCamera = canvas.worldCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                卡牌层.GetComponent<RectTransform>(),
                Input.mousePosition,
                uiCamera,
                out 本地坐标
            );
            Vector3 鼠标UI位置 = (Vector3)本地坐标;

            // 用相对Canvas尺寸的位差（如30%、20%）
            Vector2 canvasSize = ((RectTransform)canvas.transform).sizeDelta;
            Vector3 相对位差 = new Vector3(canvasSize.x * a, canvasSize.y * b, 0);

            foreach (var item in 卡牌组)
            {
                item.目标位置 = 鼠标UI位置 + 相对位差;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 胜负图 : Unity单例类<胜负图>
{
    GameObject 主胜;
    GameObject 客胜;
    bool 退出 = false;
    protected override void Awake()
    {
        场景切换时销毁 = true;
        base.Awake();
    }

    void Start()
    {
        主胜 = transform.GetChild(0).gameObject;
        客胜 = transform.GetChild(1).gameObject;
    }

    public void 胜利()
    {
        音频管理器.唯一单例.停止BGM播放();
        音频管理器.唯一单例.音效播放("过关", false);
        执子控制.唯一单例.转否 = false;
        switch (执子控制.唯一单例.当前执子者)
        {
            case 执子控制.执子者.主方:
                StartCoroutine(胜利流程(主胜, 客胜));
                break;
            case 执子控制.执子者.客方:
                StartCoroutine(胜利流程(客胜, 主胜));
                break;
        }
    }

    private void Update()
    {
        if (退出 && Input.anyKeyDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("主场景");
        }
    }

    IEnumerator 胜利流程(GameObject 胜方, GameObject 负方)
    {
        yield return StartCoroutine(弹出动画(胜方));
        负方.SetActive(false);
        退出 = true ;
    }

    IEnumerator 弹出动画(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.localScale = Vector3.zero;
        float 时间 = 0f;
        float 持续时间 = 0.3f;
        Vector3 目标缩放 = Vector3.one;
        while (时间 < 持续时间)
        {
            时间 += Time.deltaTime;
            float t = 时间 / 持续时间;
            // 使用弹性插值
            float scale = Mathf.SmoothStep(0, 1, t) + Mathf.Sin(t * Mathf.PI) * 0.1f;
            obj.transform.localScale = Vector3.one * scale;
            yield return null;
        }
        obj.transform.localScale = 目标缩放;
    }
}

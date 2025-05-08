using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class 手牌管理 : Unity单例类<手牌管理>
{
    [Header("布局配置")]
    [SerializeField] float 最小角度 = 15f;   // 1张牌时的角度
    [SerializeField] float 最大角度 = 60f;  // 最大展开角度
    [SerializeField] float 基础半径 = 3.5f;    // 基准距离
    [SerializeField] float 半径增量 = 0.1f;  // 每张牌增加的半径
    [SerializeField] float 动画时长 = 0.5f;  // 布局动画时间
    [SerializeField] Ease 缓动类型 = Ease.OutBack;

    List<棋牌基类> 当前手牌 = new();
    string[] 牌名 = { "石剪", "石布", "布剪", "石王", "布王", "剪王" };
    Vector3 布局中心点 = new(0, -4, -6.6f);

    private void Start()
    {
        foreach (var item in 牌名)
        {
            var 手牌 = 对象池.唯一单例.取出对象(item);
            if (手牌 != null) 手牌加入(手牌);
        }
    }

    public void 手牌加入(GameObject 手牌)
    {
        if (!手牌.TryGetComponent<棋牌基类>(out var 组件)) return;

        手牌.transform.position = 布局中心点;
        当前手牌.Add(组件);
        位置调整();
    }

    public void 手牌退出()
    {
        if (棋牌基类.当前放大牌 == null) return;

        棋牌基类.当前放大牌.手牌 = false;
        棋牌基类.当前放大牌.transform.rotation = Quaternion.identity;
        当前手牌.Remove(棋牌基类.当前放大牌);
        位置调整();
    }

    private void 位置调整()
    {
        int 手牌数 = 当前手牌.Count;
        if (手牌数 == 0) return;

        // 动态计算布局参数
        float 实际角度 = Mathf.Lerp(最小角度, 最大角度, Mathf.Clamp01(手牌数 / 5f));
        float 实际半径 = 基础半径 + 半径增量 * (手牌数 - 1);

        for (int i = 0; i < 手牌数; i++)
        {
            Transform 牌变换 = 当前手牌[i].transform;
            float 角度偏移 = 手牌数 == 1 ? 0 : 实际角度 * 0.5f - (实际角度 / (手牌数 - 1)) * i;
            Vector3 目标位置 = 计算牌位置(i, 角度偏移, 实际半径);

            // 修改旋转计算方式
            Quaternion 目标旋转 = Quaternion.Euler(0, 计算牌朝向角度(目标位置) - 180, 0);

            牌变换.DOKill();
            DOTween.Sequence()
                .Join(牌变换.DOMove(目标位置, 动画时长).SetEase(缓动类型))
                .Join(牌变换.DORotateQuaternion(目标旋转, 动画时长))
                .SetDelay(i * 0.05f);
        }
    }

    float 计算牌朝向角度(Vector3 牌位置)
    {
        Vector3 方向 = 布局中心点 - 牌位置;
        return Mathf.Atan2(方向.x, 方向.z) * Mathf.Rad2Deg;
    }

    Vector3 计算牌位置(int 索引, float 角度偏移, float 半径)
    {
        float 弧度 = 角度偏移 * Mathf.Deg2Rad;
        return new Vector3(
            布局中心点.x + 半径 * Mathf.Sin(弧度),
            布局中心点.y,
            布局中心点.z + 半径 * Mathf.Cos(弧度)
        );
    }
}
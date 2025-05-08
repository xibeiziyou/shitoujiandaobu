using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

class 棋牌基类 : MonoBehaviour
{

    public static 棋牌基类 当前放大牌;
    public static int 行动点数 = 1;

    private float 首次点击时间;
    private const float 双击间隔 = 0.3f;
    private Vector3 原始尺寸;
    bool 触发 = false;

    private static List<GameObject> 提示组 = new();
    public enum 牌型
    {
        石,
        剪,
        布,
        王
    }

    牌型[] 自身牌型 = new 牌型[2];

    int 激活数 = 0;

    public 牌型 激活牌型;

    public bool 手牌 = true;

    private void Start()
    {
        原始尺寸 = transform.localScale;

        自身牌型[0] = (牌型)Enum.Parse(typeof(牌型), transform.name[0].ToString());
        自身牌型[1] = (牌型)Enum.Parse(typeof(牌型), transform.name[1].ToString());

        激活牌型 = 自身牌型[激活数];
    }

    public void 翻转(UnityAction 回调 = null)
    {
        激活数 = (激活数 + 1) % 2;
        激活牌型 = 自身牌型[激活数];
        transform.DORotate(new(0, 0, 180 * 激活数), 1);
        回调?.Invoke();
    }

    private void OnMouseDown()
    {
        if (行动点数 == 0) return;
        // 即时反馈：按下缩小
        if (当前放大牌 != this) transform.DOScale(原始尺寸 * 0.8f, 0.1f);
        else 
        {
            // 双击检测
            if (Time.time - 首次点击时间 < 双击间隔 && 当前放大牌 == this)
            {
                if(!手牌)触发 = true;
                处理双击();
                return;
            }
            else 
            {
                恢复原状(() => transform.DOScale(原始尺寸 * 1.2f, 0.25f));
            }
            首次点击时间 = Time.time;
        }
        // 处理其他牌点击
        if (当前放大牌 != null && 当前放大牌 != this)
        {
            当前放大牌.恢复原状(() => { 可行动位隐藏(); });
            Debug.Log("当前放大牌复原：" + 当前放大牌.name);
            当前放大牌 = null;
        }

       if(当前放大牌 != null) Debug.Log("松开前放大牌为" + 当前放大牌.name); else { Debug.Log("放大牌为空"); }
    }

    public void 可行动位隐藏()
    {
        foreach (var item in 提示组)
        {
            对象池.唯一单例.加入对象(item);
        }
        提示组.Clear();
    }

    Tween 放大动画;

    private void OnMouseUp()
    {
        if (行动点数 == 0) return;
        if (当前放大牌 == this)
        {
            if (触发)
            {
                当前放大牌 = null;
                触发 = false;
            }
            return;
        }
        if (放大动画 != null && 放大动画.IsActive()) return;
        // 松开时恢复/放大
        放大动画 = transform.DOScale(原始尺寸 * 1.2f, 0.25f)
        .SetEase(Ease.OutBack)
        .OnComplete(() =>
        {
            当前放大牌 = this;
            可行动位显示();
        });
    }

    Vector3[] 偏移方向;

    private void 可行动位显示()
    {
        // 获取当前坐标
        Vector3 当前坐标 = transform.position;

        if (transform.position.x <= 1.5f &&
            transform.position.x >= -1.5f &&
            transform.position.z <= 1.5f &&
            transform.position.z >= -1.5f)
        {
            手牌 = false;
            偏移方向 = new Vector3[]
            {
            new Vector3(1.5f, 0, 0)+当前坐标,  // 东
            new Vector3(-1.5f, 0, 0)+当前坐标, // 西
            new Vector3(0, 0, 1.5f)+当前坐标,  // 北
            new Vector3(0, 0, -1.5f)+当前坐标  // 南
            };
        }
        else
        {
            手牌 = true;
            偏移方向 = new Vector3[]
            {
            new Vector3(1.5f, -4, -1.5f),
            new Vector3(-1.5f, -4,-1.5f),
            new Vector3(0, -4, -1.5f),
            };
        }

        // 射线参数
        float 检测距离 = 10f; // 根据场景高度调整
        Vector3 射线方向 = Vector3.down; // 从上方向下检测

        foreach (Vector3 目标坐标 in 偏移方向)
        {
            // 计算检测点坐标（保持当前Y轴高度）
            Vector3 检测点 = 目标坐标;
            检测点.y = transform.position.y;

            // 提升射线起点高度（从空中向下发射）
            Vector3 射线起点 = 检测点 + Vector3.up * 检测距离 / 2;

            // 执行射线检测
            int 检测层级 = LayerMask.GetMask("棋盘层");
            RaycastHit 碰撞信息;
            bool 检测到物体 = Physics.Raycast(
                射线起点,
                射线方向,
                out 碰撞信息,
                检测距离,
                检测层级);

            // 可视化调试
            Debug.DrawRay(射线起点, 射线方向 * 检测距离, Color.cyan, 5f);

            if (检测到物体)
            {
                Debug.Log($"在 {目标坐标} 位置检测到物体: {碰撞信息.collider.name}");
                if (碰撞信息.collider.name == "提示")
                {
                    Debug.Log("nm");
                }
            }

            // 检测到有效棋牌
            if (检测到物体 && 碰撞信息.collider.CompareTag("棋牌"))
            {
                Debug.Log($"在 {目标坐标} 位置检测到棋牌: {碰撞信息.collider.name}");
                // 在此处处理可行动位逻辑（例如高亮地面）
            }

            if (检测到物体 && 碰撞信息.collider.CompareTag("棋盘"))
            {
                GameObject 提示 = 对象池.唯一单例.取出对象("提示");
                提示.transform.position = 目标坐标;
                提示组.Add(提示);
                Debug.Log($"在 {提示.transform.position} 位置检测到棋盘: {碰撞信息.collider.name}");
            }
        }
    }

    private void 处理双击()
    {
        翻转(() => {
            if(!手牌)
            恢复原状(() => {
                可行动位隐藏();
            });
        });
    }

    public void 恢复原状(UnityAction 回调 = null)
    {
        if(回调 != null) 
        {
            transform.DOScale(原始尺寸, 0.05f)
                .SetEase(Ease.InOutQuad)
                .OnKill(() => { 回调?.Invoke(); });
        }
        transform.DOScale(原始尺寸, 0.25f)
            .SetEase(Ease.InOutQuad);
    }
}

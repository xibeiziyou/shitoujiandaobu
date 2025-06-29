using System;
using System.Collections.Generic;
using DG.Tweening;
using STJDB;
using UnityEngine;
using UnityEngine.Events;

public class 棋牌基类 : MonoBehaviour
{

    public static 棋牌基类 当前放大牌;
    public static int 行动点数 = 1;

    private float 首次点击时间;
    private const float 双击间隔 = 0.3f;
    private Vector3 原始尺寸;
    bool 触发 = false;

    public int 主客方;
    GameObject 图片1;
    GameObject 图片2;
    SpriteRenderer 标记1;
    SpriteRenderer 标记2;
    private static readonly List<GameObject> 提示组 = new();
    public enum 牌型
    {
        王,
        布,
        剪,
        石
    }

    public readonly 牌型[] 自身牌型 = new 牌型[2];

    protected int 激活数 = 0;

    public 牌型 激活牌型;

    public bool 手牌 = true;

    public void Start()
    {
        主客方 = 0;

        原始尺寸 = transform.localScale;

        自身牌型[0] = (牌型)Enum.Parse(typeof(牌型), transform.name[0].ToString());
        自身牌型[1] = (牌型)Enum.Parse(typeof(牌型), transform.name[1].ToString());

        激活牌型 = 自身牌型[激活数];

        图片1 = transform.GetChild(0).gameObject;
        图片2 = transform.GetChild(1).gameObject;
        标记1 = transform.GetChild(2).GetComponent<SpriteRenderer>();
        标记2 = transform.GetChild(3).GetComponent<SpriteRenderer>();
        牌型调整();
    }

    public void 牌型调整()
    {
        Sprite 新图片 = Resources.Load<Sprite>(自身牌型[(激活数 + 1) % 2].ToString());
        //Debug.Log(自身牌型[(激活数 + 1) % 2].ToString());
        if (新图片 != null && !执子控制.唯一单例.是否联机)
        {
            标记1.sprite = 新图片;
            标记2.sprite = 新图片;
        }

        if (激活牌型.ToString() == 图片1.name)
        {
            图片1.SetActive(true);
            图片2.SetActive(false);
        }
        else
        {
            图片2.SetActive(true);
            图片1.SetActive(false);
        }
    }

    public void 翻转(UnityAction 回调 = null)
    {
        激活数 = (激活数 + 1) % 2;
        激活牌型 = 自身牌型[激活数];
        transform.DORotate(new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            180 * 激活数), 1);
        牌型调整();
        翻转消息 翻转 = new()
        {
            牌名 = 当前放大牌.name,
            是否是手牌 = 当前放大牌.手牌
        };
        客户端.发送消息<翻转消息>(消息类型.翻转消息, 翻转);   
        if ((当前放大牌.transform.position.z == 1.5 && 主客方 == 0) || (当前放大牌.transform.position.z == -1.5 && 主客方 == 1))
            if (当前放大牌.激活牌型 == 牌型.王)
            {
                行动点数 = 0;
                Debug.Log("游戏胜利！胜利者：" + 执子控制.唯一单例.当前执子者);
                胜负图.唯一单例.胜利();
            }
        回调?.Invoke();
        if (!手牌) 执子控制.唯一单例.执子者切换();
    }

    public virtual void OnMouseDown()
    {
        if (!执子控制.唯一单例.执子判断(主客方)) return;
        if (行动点数 == 0) return;

        // 双击检测专用时间戳
        if (当前放大牌 == this)
        {
            float 时间差 = Time.time - 首次点击时间;
            if (时间差 < 双击间隔)
            {
                处理双击();
                return;
            }
        }
        else // 新点击
        {
            首次点击时间 = Time.time;
        }

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
            当前放大牌 = null;
        }
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

    public virtual void OnMouseUp()
    {
        if (!执子控制.唯一单例.执子判断(主客方)) return;
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
        音频管理器.唯一单例 .音效播放("滚动",false);
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
            new Vector3(1.5f, 0, 0)+当前坐标, 
            new Vector3(-1.5f, 0, 0)+当前坐标,
            new Vector3(0, 0, 1.5f)+当前坐标, 
            new Vector3(0, 0, -1.5f)+当前坐标
            };
        }
        else
        {
            手牌 = true;
            if (当前放大牌.主客方 == 0)
            {
                偏移方向 = new Vector3[]
                {
                new Vector3(1.5f, -4, -1.5f),
                new Vector3(-1.5f, -4,-1.5f),
                new Vector3(0, -4, -1.5f),
                };
            }
            else 
            {
                偏移方向 = new Vector3[]
                {
                new Vector3(1.5f, -4, 1.5f),
                new Vector3(-1.5f, -4,1.5f),
                new Vector3(0, -4, 1.5f),
                };
            }

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

            // 检测到有效棋牌
            if (检测到物体 && 碰撞信息.collider.CompareTag("棋牌"))
            {
                棋牌基类 目标牌 = 碰撞信息.collider.GetComponent<棋牌基类>();

                if (目标牌.手牌 || 目标牌.主客方 == 主客方) continue;
                牌型 目标牌型 = 目标牌.激活牌型;
                if (目标牌型 == 激活牌型)
                {
                    if(手牌)return;
                    GameObject 提示 = 对象池.唯一单例.取出对象("蓝");
                    提示.GetComponent<放置_蓝>().设置目标牌(目标牌);
                    提示.transform.position = 目标坐标 + new Vector3(0, 1, 0);
                    提示组.Add(提示);
                }
                else if (目标牌型 == 牌型.石 && 激活牌型 == 牌型.布)
                {
                    GameObject 提示 = 对象池.唯一单例.取出对象("绿");
                    提示.GetComponent<放置_绿>().设置目标牌(目标牌);
                    提示.transform.position = 目标坐标 + new Vector3(0, 1, 0);
                    提示组.Add(提示);
                }
                else if (目标牌型 == 牌型.布 && 激活牌型 == 牌型.石)
                {
                    GameObject 提示 = 对象池.唯一单例.取出对象("红");
                    提示.GetComponent<放置_红>().设置目标牌(目标牌);
                    提示.transform.position = 目标坐标 + new Vector3(0, 1, 0);
                    提示组.Add(提示);
                }
                else
                {
                    if ((int)目标牌型 > (int)激活牌型)
                    {
                        GameObject 提示 = 对象池.唯一单例.取出对象("红");
                        提示.GetComponent<放置_红>().设置目标牌(目标牌);
                        提示.transform.position = 目标坐标 + new Vector3(0, 1, 0);
                        提示组.Add(提示);
                    }
                    else
                    {
                        GameObject 提示 = 对象池.唯一单例.取出对象("绿");
                        提示.GetComponent<放置_绿>().设置目标牌(目标牌);
                        提示.transform.position = 目标坐标 + new Vector3(0, 1, 0);
                        提示组.Add(提示);
                    }
                }
            }

            if (检测到物体 && 碰撞信息.collider.CompareTag("棋盘"))
            {
                GameObject 提示 = 对象池.唯一单例.取出对象("提示");
                提示.transform.position = 目标坐标;
                提示组.Add(提示);
                //Debug.Log($"在 {提示.transform.position} 位置检测到棋盘: {碰撞信息.collider.name}");
            }
        }
    }

    private void 处理双击()
    {
        if (触发) return; // 防止重复触发
        触发 = true;

        翻转(() => {
            当前放大牌 = null;
            首次点击时间 = 0;
            触发 = false;

            if (!手牌)
            {
                恢复原状(() => 可行动位隐藏());
            }
            else
            {
                transform.DOScale(原始尺寸, 0.1f);
            }
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

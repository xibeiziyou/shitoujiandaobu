using System.Collections.Generic;
using UnityEngine.Events;

public class 事件中心 : 单例类<事件中心>
{
    private readonly Dictionary<string, List<(int 优先级, UnityAction 方法)>> 事件库 = new();
    private readonly Dictionary<string, List<(int 优先级, UnityAction 方法)>> 禁用事件库 = new();

    // 增加事件监听（带优先级）
    public void 增加事件监听(string 事件名, UnityAction 方法, int 优先级 = 0)
    {
        if (事件库.ContainsKey(事件名))
        {
            // 添加到事件列表
            事件库[事件名].Add((优先级, 方法));
            // 按优先级排序（优先级高的先执行）
            事件库[事件名].Sort((a, b) => b.优先级.CompareTo(a.优先级));
        }
        else
        {
            // 创建新的事件列表
            事件库.Add(事件名, new List<(int, UnityAction)> { (优先级, 方法) });
        }
    }

    // 移除事件监听
    public void 事件方法移除(string 事件名, UnityAction 方法)
    {
        if (事件库.ContainsKey(事件名))
        {
            // 找到并移除对应的方法
            var 事件列表 = 事件库[事件名];
            事件列表.RemoveAll(item => item.方法 == 方法);

            // 如果事件列表为空，移除事件
            if (事件列表.Count == 0)
            {
                事件库.Remove(事件名);
            }
        }
    }

    public void 事件移除(string 事件名)
    {
        if (事件库.ContainsKey(事件名))
        {
            事件库.Remove(事件名);
        }
    }

    public void 事件禁用(string 事件名)
    {
        if (事件库.ContainsKey(事件名))
        {
            禁用事件库.Add(事件名, 事件库[事件名]);
            事件库.Remove(事件名);
        }
    }

    public void 事件启用(string 事件名)
    {
        if (禁用事件库.ContainsKey(事件名))
        {
            事件库.Add(事件名, 禁用事件库[事件名]);
            禁用事件库.Remove(事件名);
        }
    }

    // 触发事件
    public void 事件触发(string 事件名)
    {
        if (事件库.ContainsKey(事件名))
        {
            //Debug.Log(事件名);
            // 按优先级顺序执行
            foreach (var (优先级, 方法) in 事件库[事件名])
            {
                //Debug.Log($"执行方法: {方法.Method.Name}, 优先级: {优先级}");
                方法?.Invoke();
            }
        }
    }

    int 事件数 = 0;
    public void 事件同引用时间触发(string 事件名,int 事件引用数) 
    {
        if (!事件库.ContainsKey(事件名))return;

        事件数++;

        if(事件数 == 事件引用数) 
        {
            foreach (var (优先级, 方法) in 事件库[事件名])
            {
                方法?.Invoke();
            }
            事件数 = 0;
        }
    }

    // 清空事件库
    public void 清空()
    {
        事件库.Clear();
    }
}
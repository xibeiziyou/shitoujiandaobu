using System.Collections.Generic;
using UnityEngine;

public class 主线程调度器 : Unity单例类<主线程调度器>
{
    private static readonly Queue<System.Action> 执行队列 = new();

    //后台线程通过Enqueue添加任务
    public void 任务入队(System.Action action)
    {
        lock (执行队列)
        {
            执行队列.Enqueue(action);
        }
    }

    //主线程在每帧Update中执行队列任务
    void Update()
    {
        lock (执行队列)
        {
            while (执行队列.Count > 0)
            {
                执行队列.Dequeue()?.Invoke();
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum 面板类型
{
    开始面板,
    设置面板,
    游戏面板,
    排行面板,
    退出面板,
    附加面板
}

public class UI管理器 : 单例类<UI管理器>
{

    private Dictionary<面板类型, 面板基类> 面板字典 = new Dictionary<面板类型, 面板基类>();
    private Stack<面板基类> 面板栈 = new Stack<面板基类>();
    Transform 画布 = GameObject.Find("Canvas").transform;

    private 面板基类 面板查找(面板类型 面板)
    {
        if (面板字典.ContainsKey(面板))
        {
            return 面板字典[面板];
        }
        else
        {
            面板基类 面板实例 = GameObject.Instantiate(Resources.Load<GameObject>("UI界面/" + 面板.ToString()).GetComponent<面板基类>());
            if (面板实例)
            {
                面板实例.transform.SetParent(画布, false);
                面板字典.Add(面板, 面板实例);
                return 面板实例;
            }
            else
            {
                Debug.LogError("没有找到面板" + 面板.ToString());
                return null;
            }
        }
    }

    public void 面板弹出(面板类型 面板, UnityAction<面板基类> 回调 = null)
    {
        面板基类 面板实例 = 面板查找(面板);
        if (面板实例 != null)
        {
            if (面板栈.Count > 0)
            {
                面板基类 顶部面板 = 面板栈.Peek();
                顶部面板.面板禁用();
            }
            面板实例.面板显示();
            面板栈.Push(面板实例);
        }

        回调?.Invoke(面板实例);
    }

    public void 面板消退()
    {
        if (面板栈.Count > 0)
        {
            面板基类 顶部面板 = 面板栈.Pop();
            顶部面板.面板隐藏();
            if (面板栈.Count > 0)
            {
                面板基类 新顶部面板 = 面板栈.Peek();
                新顶部面板.面板启用();
            }
        }
    }

    public void 面板清空()
    {
        while (面板栈.Count > 0)
        {
            面板基类 顶部面板 = 面板栈.Pop();
            顶部面板.面板隐藏();
        }
        面板字典.Clear();
    }

    public bool 面板检视(面板类型 类型)
    {
        foreach (Transform item in 画布.transform)
        {
            if (item.name == 类型.ToString() + "(Clone)" && item.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}

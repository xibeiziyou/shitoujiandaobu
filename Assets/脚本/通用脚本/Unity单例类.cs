using UnityEngine;

public abstract class Unity单例类<T> : MonoBehaviour where T : MonoBehaviour
{
    // 实例控制字段
    private static T 私有实例;
    private static readonly object 线程锁 = new();
    private static bool 应用退出标记 = false;

    private static bool _场景切换时销毁 = false;
    public static bool 场景切换时销毁
    {
        get => _场景切换时销毁;
        set => _场景切换时销毁 = value;
    }

    // 公开访问属性
    public static T 唯一单例
    {
        get
        {
            if (应用退出标记)
            {
                Debug.LogWarning($"单例 {typeof(T)} 已被销毁，不再创建新实例");
                return null;
            }

            lock (线程锁)
            {
                if (私有实例 == null)
                {
                    私有实例 = FindObjectOfType<T>();
                    if (私有实例 == null)
                    {
                        GameObject 单例载体对象 = new GameObject($"{typeof(T)}_单例");
                        私有实例 = 单例载体对象.AddComponent<T>();
                        if (!_场景切换时销毁)
                        {
                            DontDestroyOnLoad(单例载体对象);
                        }
                    }
                }
                return 私有实例;
            }
        }
    }

    // 生命周期方法
    protected virtual void Awake()
    {
        if (私有实例 != null && 私有实例 != this)
        {
            Destroy(gameObject);
        }
        else
        {
            私有实例 = this as T;
            if (!_场景切换时销毁)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    protected virtual void OnApplicationQuit()
    {
        应用退出标记 = true;
    }
}
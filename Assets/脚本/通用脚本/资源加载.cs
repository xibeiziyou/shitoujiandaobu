using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class 资源加载 : Unity单例类<资源加载>
{

    public T 同步加载<T>(string 名字) where T : Object
    {
        T 资源 = Resources.Load<T>(名字);
        if(资源 is GameObject) 
        {
            return GameObject.Instantiate(资源);
        }
        else 
        {
            return 资源;
        }
    }

    public void 异步加载<T>(string 名字, UnityAction<T> 回调函数) where T : Object
    {
        StartCoroutine(资源异步加载(名字, 回调函数));
    }

    private IEnumerator 资源异步加载<T>(string 名字, UnityAction<T> 回调函数) where T : Object
    {
        ResourceRequest 请求 = Resources.LoadAsync<T>(名字);
        yield return 请求; 

        T 资源 = 请求.asset as T;

        if (资源 is GameObject)
        {
            回调函数(GameObject.Instantiate(资源));
        }
        else
        回调函数.Invoke(资源);
    }
}

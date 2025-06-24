using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 标记控制 : MonoBehaviour
{
    GameObject 图片隐;
    GameObject 图片显;

    private readonly List<GameObject> 图片集 = new();

    public bool 是否显隐;

    private void Awake()
    {

        图片隐 = transform.GetChild(0).gameObject;
        图片显 = transform.GetChild(1).gameObject;

        事件中心.唯一单例.增加事件监听("标记获取", 标记初始化);
    }

    public void 标记初始化() 
    {
        GameObject[] 标记主物体 = GameObject.FindGameObjectsWithTag(this.name);
        //Debug.Log(标记主物体.Length);
        foreach (var obj in 标记主物体)
        {
            图片集.Add(obj);
        }
        if (是否显隐)
        {
            图片隐.SetActive(false); 图片显.SetActive(true);
            foreach (var item in 图片集)
            {
                item.SetActive(true);
            }
        }
        else
        {
            图片隐.SetActive(true); 图片显.SetActive(false);
            foreach (var item in 图片集)
            {
                item.SetActive(false);
            }
        }
    }

    private void OnMouseUp() 
    {
        显隐切换();
    }

    void 显隐切换() 
    {
        //Debug.Log("按下按钮了");
        是否显隐 = !是否显隐;
        if (是否显隐) 
        {
            图片隐.SetActive(false);图片显.SetActive(true);
            foreach (var item in 图片集)
            {
                item.SetActive(true);
            }
        } 
        else
        {
            图片隐.SetActive(true); 图片显.SetActive(false);
            foreach (var item in 图片集)
            {
                item.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        事件中心.唯一单例.事件方法移除("获取标记", 标记初始化);
    }

}

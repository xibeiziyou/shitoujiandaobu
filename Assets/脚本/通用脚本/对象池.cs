using System.Collections.Generic;
using UnityEngine;

public class 对象池 : 单例类<对象池>
{
    public GameObject 池;

    public Dictionary<string ,List<GameObject>> 对象库 = new Dictionary<string ,List<GameObject>>();

    public void 加入对象(GameObject 对象)
    {
        if (对象 == null)
        {
            Debug.LogWarning("尝试将null对象加入对象池");
            return;
        }
        对象.SetActive(false);
        if (池)
        {
            对象.transform.parent = 池.transform;
        }
        else 
        {
            池 = new GameObject("对象池");
        }
        if (对象库.ContainsKey(对象.name))
        {
            对象库[对象.name].Add(对象);
        }
        else 
        {
            对象库.Add(对象.name, new List<GameObject>() { 对象 });
        }
    }

    public GameObject 取出对象(string 对象名)
    {
        GameObject 对象;
        if ( 对象库.ContainsKey(对象名) && 对象库[对象名].Count > 0)
        {
            对象 = 对象库[对象名][0];
            对象库[对象名].RemoveAt(0);
        }
        else
        {
            对象 = GameObject.Instantiate(Resources.Load<GameObject>(对象名));
            对象.name = 对象名;
        }
        对象.SetActive(true);
        return 对象;
    }

    public void 清空()
    {
        对象库.Clear();
        池 = null;
    }
}

using UnityEngine;

public class 联机相机控制 : Unity单例类<联机相机控制>
{
    protected override void Awake()
    {
        场景切换时销毁 = true;
        base.Awake();
    }

    public void 位置移动(Vector3 目标位置, float 移动速度 = 5f)
    {
        StopAllCoroutines();
        StartCoroutine(移动到位置协程(目标位置, 移动速度));
    }

    private System.Collections.IEnumerator 移动到位置协程(Vector3 目标位置, float 移动速度)
    {
        Transform 相机变换 = Camera.main.transform;
        float 距离阈值 = 0.01f;
        while (true)
        {
            float 距离 = Vector3.Distance(相机变换.position, 目标位置);
            if (距离 <= 距离阈值)
                break;
            float 插值因子 = 1f - Mathf.Exp(-移动速度 * Time.deltaTime); 
            相机变换.position = Vector3.Lerp(相机变换.position, 目标位置, 插值因子);
            yield return null;
        }
        相机变换.position = 目标位置;
    }
}

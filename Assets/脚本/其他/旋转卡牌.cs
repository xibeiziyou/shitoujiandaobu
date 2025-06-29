using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 旋转卡牌 : MonoBehaviour
{
    public float 自转速度;
    public Vector3 目标位置;
    public float 移动速度;

    Image 图片;
    private void Start()
    {
        目标位置 = new Vector3(Random.Range(-100, 1000), Random.Range(-100, 700), 0);

        图片 = GetComponent<Image>();

        自转速度 = Random.Range(90, 180);
        移动速度 = 自转速度 / 90f * 50f;

        int 图层编号 = Random.Range(1, 6);
        if (图层编号 == 5) 图层编号 = 8;
        Sprite 图层图片 = Resources.Load<Sprite>($"图层 {图层编号}");
        图片.sprite = 图层图片;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, 自转速度 * Time.deltaTime);

        
        float distance = Vector3.Distance(transform.position, 目标位置);
        if (distance > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, 目标位置, 移动速度 * Time.deltaTime);
        }
        else 
        {
            目标位置 = new Vector3(Random.Range(-100,1000), Random.Range(-100, 700),0);
        }
    }
}

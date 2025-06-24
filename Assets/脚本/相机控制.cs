using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class 相机控制 : Unity单例类<相机控制>
{
    Camera 相机;

    HashSet<string> 层集;

    // 新增：记录动画Tween
    private Tween 旋转Tween;
    private Tween 移动Tween;

    private void Start()
    {
        层集 = new HashSet<string>();
        for (int i = 0; i < 32; i++)
        {
            string 层级名 = LayerMask.LayerToName(i);
            if (!string.IsNullOrEmpty(层级名))
            {
                层集.Add(层级名);
            }
        }
        
        相机 = GetComponent<Camera>();
    }

    public void 相机旋转()
    {
        // 如果动画正在进行，立刻完成
        if (旋转Tween != null && 旋转Tween.IsActive())
        {
            旋转Tween.Complete();
        }
        if (移动Tween != null && 移动Tween.IsActive())
        {
            移动Tween.Complete();
        }

        // 动画旋转
        旋转Tween = transform.DORotate(new Vector3(0, 180, 0), 2f, RotateMode.WorldAxisAdd);

        // 动画移动
        Vector3 目标位置 = transform.position;
        
        //目标位置.z = 目标位置.z > 0 ? 1 : -1;
        目标位置.z = -目标位置.z;
        移动Tween = transform.DOMove(目标位置, 0.5f);

        切换剔除遮罩();
    }

    private void 切换剔除遮罩()
    {
        if (层集.Contains("主") && 层集.Contains("客") || !层集.Contains("主") && !层集.Contains("客"))
        {
            if (游戏控制.唯一单例.当前执子者 == 游戏控制.执子者.主方)
            {
                层集.Remove("客");
                层集.Add("主");
            }
            else
            {
                层集.Remove("主");
                层集.Add("客");
            }
        }
        else if (层集.Contains("客") && !层集.Contains("主"))
        {
            层集.Remove("客");
            层集.Add("主");
        }
        else if (!层集.Contains("客") && 层集.Contains("主"))
        {
            层集.Remove("主");
            层集.Add("客");
        }

        if (层集 == null || 层集.Count == 0)
            return;

        int mask = LayerMask.GetMask(层集.ToArray());
        相机.cullingMask = mask;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 对话框 : MonoBehaviour
{
    [SerializeField] private TMP_Text 字;

    public void 字体设置(string 内容)
    {
        字.text = 内容;
        字.ForceMeshUpdate();
        float textWidth = 字.preferredWidth;
        float 最小 = 100f;
        float 最大 = 600f;
        float 间距 = 40f;
        float newWidth = Mathf.Clamp(textWidth + 间距, 最小, 最大);

        if (TryGetComponent<LayoutElement>(out var layout))
        {
            layout.preferredWidth = newWidth;
        }
    }
}

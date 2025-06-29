using TMPro;
using UnityEngine;

public class 主命数 : MonoBehaviour
{

    public static TMP_Text 命数显示;

    public static int 命数;
    private void Start()
    {    
        命数显示 = transform.GetChild(1).GetComponent<TMP_Text>();

        命数 = 3;

        命数显示.text = 命数.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 设置面板 : 面板基类
{
    Button 返回;
    Slider 音乐;
    Slider 音效;

    Image 乐;
    Image 效;

    void Start()
    {
        返回 = transform.GetChild(2).GetComponent<Button>();
        音乐 = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        乐 = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        音效 = transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        效 = transform.GetChild(1).GetChild(1).GetComponent<Image>();

        音乐.value = PlayerPrefs.GetFloat("音乐音量", 1f);
        音效.value = PlayerPrefs.GetFloat("音效音量", 1f);

        返回.onClick.AddListener(() =>
        {
            UI管理器.唯一单例.面板消退();
        });

        音乐.onValueChanged.AddListener((值) =>
        {
            Color color = 乐.color;
            color.r = 值;
            乐.color = color;

            音频管理器.唯一单例.更改BGM音量大小(值);
        });

        音效.onValueChanged.AddListener((值) =>
        {
            Color color = 效.color;
            color.r = 值;
            效.color = color;

            音频管理器.唯一单例.改变音效音量(值);
        });
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("音乐音量", 音乐.value);
        PlayerPrefs.SetFloat("音效音量", 音效.value);
        PlayerPrefs.Save();
    }
}

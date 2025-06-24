using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class 音频管理器 : Unity单例类<音频管理器>
{

    private AudioSource BGM;

    private float BGM音量大小 = 1;

    private GameObject 音效;

    private readonly List<AudioSource> 音效列表 = new();

    private float 音效音量 = 1;

    public void 播放BGM(string 音乐名) 
    {
        if (BGM == null) 
        {
            GameObject 载体 = new();
            载体.name = "BGM";
            BGM = 载体.AddComponent<AudioSource>();
        }

        资源加载.唯一单例.异步加载<AudioClip>("音频/BGM/" + 音乐名, (音源) =>
        {
            BGM.clip = 音源;
            BGM.loop = true;
            BGM.volume = BGM音量大小;
            BGM.Play();
        });
    }

    public void 暂停BGM播放() 
    {
        if (BGM == null) return;
        BGM.Pause();
    }

    public void 更改BGM音量大小(float 值) 
    {
        BGM音量大小 = 值;
        if (BGM == null) return;
        BGM.volume = BGM音量大小;
    }

    public void 停止BGM播放() 
    {
        if (BGM == null) return;
        BGM.Stop();
    }

    public void 音效播放(string 音效名, bool 是否循环, UnityAction<AudioSource> 回调 = null)
    {
        if (音效 == null)
        {
            音效 = new()
            {
                name = "音效"
            };
        }

        资源加载.唯一单例.异步加载<AudioClip>("音频/音效/" + 音效名, (音源) => 
        {
            AudioSource 音 = 音效.AddComponent<AudioSource>();
            音.clip = 音源;
            音.loop = 是否循环;
            音.volume = 音效音量;
            音.Play();
            音效列表.Add(音);
            回调?.Invoke(音);
        });
    }

    public void 停止音效(AudioSource 指定音效) 
    {
        if (音效列表.Contains(指定音效)) 
        {
            音效列表.Remove(指定音效);
            指定音效.Stop();
            Destroy(指定音效);
        }
    }

    public void 改变音效音量(float 音量) 
    {
        音效音量 = 音量;
        foreach (var item in 音效列表)
        {
            item.volume = 音效音量;
        }
    }

    private void Update()
    {
        for (int i = 音效列表.Count - 1; i >= 0; i--)
        {
            if (音效列表[i] == null || !音效列表[i].isPlaying)
            {
                if (音效列表[i] != null)
                {
                    Destroy(音效列表[i]);
                }
                音效列表.RemoveAt(i);
            }
        }
    }
}

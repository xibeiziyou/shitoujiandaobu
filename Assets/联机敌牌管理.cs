using STJDB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 联机敌牌管理 : MonoBehaviour
{
    private readonly Dictionary<string, GameObject> 牌组 = new();
    private readonly string[] 牌名 = { "石剪-敌", "石布-敌", "布剪-敌", "石王-敌", "布王-敌", "剪王-敌" };
    void Start()
    {
        音频管理器.唯一单例.播放BGM("bgm");

        foreach (var item in 牌名)
        {
            GameObject 牌 = 对象池.唯一单例.取出对象(item);
            牌.transform.SetParent(transform, false);
            Destroy(牌.transform.GetChild(2).gameObject);
            Destroy(牌.transform.GetChild(3).gameObject);
            牌组.Add(item, 牌);
        }

        客户端.收到消息 += 消息处理;
    }
    通用消息包 数据包;
    void 消息处理(byte[] 数据)
    {
        数据包 = 协议工具类.解包(数据);
        //Debug.Log("收到消息啦");
        switch (数据包.类型) 
        {
            case 消息类型.移动消息:
                {
                    移动消息 移动 = 协议工具类.还原<移动消息>(数据包.数据);
                    // Explicitly convert System.Numerics.Vector3 to UnityEngine.Vector3
                    牌组[移动.牌名 + "-敌"].transform.position = new UnityEngine.Vector3(移动.移动位置.X, 移动.移动位置.Y, -移动.移动位置.Z);
                    牌组[移动.牌名 + "-敌"].GetComponent<敌牌>().手牌 = false;
                    执子控制.唯一单例.执子者切换();
                }
                break;
            case 消息类型.翻转消息: 
                {
                    翻转消息 翻转 = 协议工具类.还原<翻转消息>(数据包.数据);
                    牌组[翻转.牌名 + "-敌"].GetComponent<敌牌>().卡牌转换();
                    if( !翻转.是否是手牌) 执子控制.唯一单例.执子者切换();
                }
                break;
            case 消息类型.吃牌消息:
                {
                    吃牌消息 吃牌 = 协议工具类.还原<吃牌消息>(数据包.数据);
                    // Replace the problematic line with an explicit conversion from System.Numerics.Vector3 to UnityEngine.Vector3
                    牌组[吃牌.牌名 + "-敌"].transform.position = new UnityEngine.Vector3(吃牌.被吃位置.X, 吃牌.被吃位置.Y, -吃牌.被吃位置.Z);
                    牌组[吃牌.牌名 + "-敌"].GetComponent<敌牌>().手牌 = false;
                    string 牌 = 吃牌.被吃牌名.Replace("-敌", "");
                    Debug.Log(牌);
                    GameObject pai = GameObject.Find(牌);
                    if (牌.Contains("王"))
                    {
                        主命数.命数--;
                        if (主命数.命数显示) 主命数.命数显示.text = 主命数.命数.ToString();
                        if (主命数.命数 == 0)
                        {
                            棋牌基类.行动点数 = 0;
                            Debug.Log("客胜");
                        }
                        对象池.唯一单例.加入对象(pai);
                    }
                    else 
                    {
                        主手牌管理.唯一单例.手牌加入(pai);
                    }
                    执子控制.唯一单例.执子者切换();
                }
                break;
            case 消息类型.对冲消息:
                {
                    对冲消息 对冲 = 协议工具类.还原<对冲消息>(数据包.数据);
                    牌组[对冲.牌名 + "-敌"].transform.position = new(0, 0, 10);
                    牌组[对冲.牌名 + "-敌"].GetComponent<敌牌>().手牌 = true;
                    主手牌管理.唯一单例.手牌加入(GameObject.Find(对冲.对冲牌名.Replace("-敌", "")));
                    执子控制.唯一单例.执子者切换();
                }
                break;
            case 消息类型.被冲消息:
                {
                    被冲消息 被冲 = 协议工具类.还原<被冲消息>(数据包.数据);
                    牌组[被冲.牌名 + "-敌"].transform.position = new(0, 0, 10);
                    牌组[被冲.牌名 + "-敌"].GetComponent<敌牌>().手牌 = true;
                    执子控制.唯一单例.执子者切换();
                }
                break;
        }
    }
}

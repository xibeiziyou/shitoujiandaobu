using STJDB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 联机面板 : 面板基类
{
    public TMP_InputField 连接服务器;
    public ScrollRect 滚动视图;
    public RectTransform 内容区域;
    public TMP_InputField 输入字段;
    public string 对话框 = "对话框";
    Button 返回;
    private readonly 聊天消息 聊天消息 = new();
    bool 主流执子切换 = false;

    void Start()
    {
        执子控制.唯一单例.是否联机 = true;
        执子控制.唯一单例.当前执子者 = 执子控制.执子者.客方;
        连接服务器 = transform.GetChild(0).GetComponent<TMP_InputField>();

        滚动视图 = transform.GetChild(1).GetComponent<ScrollRect>();
        内容区域 = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        
        // 确保内容区域有垂直布局组件
        if (!内容区域.GetComponent<VerticalLayoutGroup>())
        {
            var 布局组 = 内容区域.gameObject.AddComponent<VerticalLayoutGroup>();
            布局组.spacing = 5f; // 消息间距
            布局组.padding = new RectOffset(10, 10, 10, 10); // 内边距
            布局组.childControlHeight = true; // 子对象控制高度
            布局组.childControlWidth = true;  // 子对象控制宽度
            布局组.childForceExpandHeight = false; // 不强制扩展高度
            布局组.childForceExpandWidth = false;  // 不强制扩展宽度
        }
        
        // 确保内容区域有ContentSizeFitter
        if (!内容区域.GetComponent<ContentSizeFitter>())
        {
            var 尺寸适配器 = 内容区域.gameObject.AddComponent<ContentSizeFitter>();
            尺寸适配器.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        
        返回 = transform.GetChild(3).GetComponent<Button>();
        返回.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("主场景");
        });
        客户端.收到消息 += 消息打印;

        输入字段 = transform.GetChild(2).GetComponent<TMP_InputField>();
        输入字段.onEndEdit.AddListener((值) =>
        {
            聊天消息.消息 = 值;
            聊天信息发送(聊天消息);
        });

        连接服务器.onEndEdit.AddListener((值) =>
        {
            if (客户端.连接服务器(值))
            {
                滚动视图.gameObject.SetActive(true);
                输入字段.gameObject.SetActive(true);
                连接服务器.gameObject.SetActive(false);

                联机相机控制.唯一单例.位置移动(new Vector3(0,0,-1));
            }

        });
    }
    通用消息包 数据包;
    void 消息打印(byte[] 数据)
    {
        数据包 = 协议工具类.解包(数据);
        //Debug.Log($"收到消息包类型: {数据包.类型}, 数据长度: {数据.Length}");
        if (数据包.类型 is 消息类型.聊天消息)
        {
            聊天消息 聊天 = 协议工具类.还原<聊天消息>(数据包.数据);
            //Debug.Log($"收到聊天内容: {聊天.消息}");
            if (聊天.消息 == "执子者切换" && !主流执子切换) 
            {
                执子控制.唯一单例.执子者切换();
                主流执子切换 = true;
                return;
            }
            添加聊天消息(聊天.消息);
        }
    }

    void 聊天信息发送(聊天消息 消息)
    {
        if (!string.IsNullOrWhiteSpace(消息.消息))
        {
            客户端.发送消息<聊天消息>(消息类型.聊天消息, 消息);
            输入字段.text = "";
            输入字段.ActivateInputField();

            添加聊天消息(消息.消息);
        }
    }
    
    void 添加聊天消息(string 消息内容)
    {
        GameObject 框 = 对象池.唯一单例.取出对象(对话框);
        框.transform.SetParent(内容区域, false);
        框.GetComponent<对话框>().字体设置(消息内容);
        
        // 强制更新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(内容区域);
        
        // 延迟一帧后滚动到底部，确保布局完成
        StartCoroutine(延迟滚动到底部());
    }
    
    System.Collections.IEnumerator 延迟滚动到底部()
    {
        yield return null; // 等待一帧
        Canvas.ForceUpdateCanvases();
        滚动视图.verticalNormalizedPosition = 0f;
    }
}

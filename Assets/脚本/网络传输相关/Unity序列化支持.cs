using MessagePack;
using MessagePack.Unity;
using UnityEngine;

public static class Unity序列化支持
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        // 注册 Unity 原生类型解析器（如 Vector3、Color 等）
        MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard
            .WithResolver(UnityResolver.InstanceWithStandardResolver);

        Debug.Log("MessagePack 初始化完成！");
    }
}
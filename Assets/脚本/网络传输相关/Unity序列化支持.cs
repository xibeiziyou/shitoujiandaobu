using MessagePack;
using MessagePack.Unity;
using UnityEngine;

public static class Unity���л�֧��
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        // ע�� Unity ԭ�����ͽ��������� Vector3��Color �ȣ�
        MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard
            .WithResolver(UnityResolver.InstanceWithStandardResolver);

        Debug.Log("MessagePack ��ʼ����ɣ�");
    }
}
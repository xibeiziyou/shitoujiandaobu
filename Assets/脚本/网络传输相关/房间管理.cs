using STJDBЭ�����;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ����
{
    public int �����;
    public List<NetworkStream> ����б� = new List<NetworkStream>();
    public ��������[,] ���� = new ��������[15, 15]; // ������15x15����
}

public static class �������
{
    public static Dictionary<int, ����> ���з��� = new Dictionary<int, ����>();
    private static int ��ǰ��󷿼�� = 1;

    public static int ��������(NetworkStream ���)
    {
        var �·��� = new ����
        {
            ����� = ��ǰ��󷿼��++,
            ���� = new ��������[15, 15]
        };
        ���з���.Add(�·���.�����, �·���);
        return �·���.�����;
    }

    public static bool ���뷿��(int �����, NetworkStream ���)
    {
        if (!���з���.ContainsKey(�����)) return false;

        var ���� = ���з���[�����];
        if (����.����б�.Count >= 2) return false;

        ����.����б�.Add(���);
        return true;
    }
}
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class ����_�� : MonoBehaviour
{
    Vector3 �ϲ�λ��;

    public ���ƻ��� Ŀ����;

    private void OnMouseDown()
    {
        ���ƻ���.��ǰ�Ŵ���.���ж�λ����();
        Ŀ����.transform.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        �ϲ�λ�� = (���ƻ���.��ǰ�Ŵ���.transform.position + Ŀ����.transform.position) / 2;
        ���ƻ���.��ǰ�Ŵ���.transform.DOMove(�ϲ�λ��, 1);
        Ŀ����.transform.DOMove(�ϲ�λ��, 1).OnKill(() =>
        {
            �ص�����(���ƻ���.��ǰ�Ŵ���.GetComponent<���ƻ���>());
            �ص�����(Ŀ����);
            ���ƻ���.��ǰ�Ŵ��� = null; Debug.Log("��ǰ�Ŵ��Ʊ�ɿ�");
            ��Ϸ����.Ψһ����.ִ�����л�();
        });
    }
    void �ص�����(���ƻ��� ��) 
    {
        if (��.���ͷ� == 0) 
        {
            �����ƹ���.Ψһ����.���Ƽ���(��.gameObject);
        }
        else 
        {
            �����ƹ���.Ψһ����.���Ƽ���(��.gameObject);
        }
    }

    public void ����Ŀ����( ���ƻ��� ��) 
    {
        Ŀ���� = ��;
        Debug.Log(Ŀ����);
    }
}

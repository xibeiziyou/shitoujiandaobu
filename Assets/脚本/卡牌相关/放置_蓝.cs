using DG.Tweening;
using STJDB;
using UnityEngine;

public class ����_�� : MonoBehaviour
{

    public ���ƻ��� Ŀ����;
    private readonly �Գ���Ϣ �Գ� = new();
    private void OnMouseDown()
    {
        ��Ƶ������.Ψһ����.��Ч����("����", false);
        ���ƻ���.��ǰ�Ŵ���.���ж�λ����();
        Ŀ����.transform.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        �ص�����(���ƻ���.��ǰ�Ŵ���.GetComponent<���ƻ���>());
        if (ִ�ӿ���.Ψһ����.�Ƿ�����)
        {
            �Գ�.���� = ���ƻ���.��ǰ�Ŵ���.name;
            �Գ�.�Գ����� = Ŀ����.name;
            �ͻ���.������Ϣ<�Գ���Ϣ>(��Ϣ����.�Գ���Ϣ, �Գ�);
            Ŀ����.transform.position = new Vector3(0, 0, 10);
        }
        else
        {
            �ص�����(Ŀ����);
        }
        ���ƻ���.��ǰ�Ŵ��� = null; //Debug.Log("��ǰ�Ŵ��Ʊ�ɿ�");
        ִ�ӿ���.Ψһ����.ִ�����л�();
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

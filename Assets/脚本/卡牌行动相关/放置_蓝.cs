using DG.Tweening;
using UnityEngine;

public class ����_�� : MonoBehaviour
{

    public ���ƻ��� Ŀ����;

    private void OnMouseDown()
    {
        ���ƻ���.��ǰ�Ŵ���.���ж�λ����();
        Ŀ����.transform.DOScale(Vector3.one * 1.2f, 0.25f).SetEase(Ease.OutBack);
        �ص�����(���ƻ���.��ǰ�Ŵ���.GetComponent<���ƻ���>());
        �ص�����(Ŀ����);
        ���ƻ���.��ǰ�Ŵ��� = null; //Debug.Log("��ǰ�Ŵ��Ʊ�ɿ�");
        ��Ϸ����.Ψһ����.ִ�����л�();
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

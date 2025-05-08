using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 面板基类 : MonoBehaviour
{
    public CanvasGroup 面板;

    private void Awake() { 面板 = gameObject.AddComponent<CanvasGroup>(); }

    public virtual void 面板显示() { gameObject.SetActive(true); }

    public virtual void 面板启用() { 面板.interactable = true; }

    public virtual void 面板禁用() { 面板.interactable = false; }

    public virtual void 面板隐藏() { gameObject.SetActive(false); }

}

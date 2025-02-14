using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ViewBase : MonoBehaviour
{
    internal CanvasGroup CanvasGroup;

    protected virtual void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetViewAlpha(float pValue)
    {
        CanvasGroup.alpha = pValue;
        SetCanvasGroupDetection(pValue == 1);
    }

    internal void SetCanvasGroupDetection(bool pState)
    {
        CanvasGroup.blocksRaycasts = pState;
        CanvasGroup.interactable = pState;
    }
}

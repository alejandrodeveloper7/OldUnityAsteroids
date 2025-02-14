using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    protected ViewBase BaseView;

    protected virtual void Awake()
    {
        BaseView = GetComponent<ViewBase>();
        Initialize();
    }

    protected abstract void Initialize();
}

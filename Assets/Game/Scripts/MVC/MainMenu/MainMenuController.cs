using UnityEngine;

[RequireComponent(typeof(MainMenuView))]
public class MainMenuController : ControllerBase
{
    [SerializeField] private MainMenuModel _model;
    private MainMenuView _view;

    protected override void Initialize()
    {
        _view = GetComponent<MainMenuView>();
    }
}

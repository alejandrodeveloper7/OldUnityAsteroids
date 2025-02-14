using UnityEngine;

[RequireComponent(typeof(GameEndView))]
public class GameEndController : ControllerBase
{
    [SerializeField] private GameEndModel _model;
    private GameEndView _view;

    protected override void Awake()
    {
        base.Awake();
        _view = GetComponent<GameEndView>();
    }

    protected override void Initialize()
    {
        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);
    }
}

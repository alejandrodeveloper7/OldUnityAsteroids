using UnityEngine;

[RequireComponent(typeof(GameStatsView))]
public class GameStatsController : ControllerBase
{
    [SerializeField] private GameStatsModel _model;
    private GameStatsView _view;

    protected override void Awake()
    {
        base.Awake();
        _view = GetComponent<GameStatsView>();
    }

    protected override void Initialize()
    {
        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);
    }
}

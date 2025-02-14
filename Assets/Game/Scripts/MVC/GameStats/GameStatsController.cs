using UnityEngine;

[RequireComponent(typeof(GameStatsView))]
public class GameStatsController : ControllerBase
{
    [SerializeField] private GameStatsModel _model;
    private GameStatsView _view;

    protected override void Initialize()
    {
        _view = GetComponent<GameStatsView>();
    }
}

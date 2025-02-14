using UnityEngine;

[RequireComponent(typeof(GameEndView))]
public class GameEndController : ControllerBase
{
    [SerializeField] private GameEndModel _model;
    private GameEndView _view;

    protected override void Initialize()
    {
        _view = GetComponent<GameEndView>();
    }
}

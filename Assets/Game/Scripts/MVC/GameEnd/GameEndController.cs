using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(GameEndView))]
public class GameEndController : ControllerBase
{
    [SerializeField] private GameEndModel _model;
    private GameEndView _view;

    #region Monobehaviour

    protected override void Awake()
    {
        base.Awake();
        _view = GetComponent<GameEndView>();
    }

    private void OnEnable()
    {
        EventManager.OnStageFinished += StageFinished;
    }

    private void OnDisable()
    {
        EventManager.OnStageFinished -= StageFinished;
    }

    #endregion

    #region Event Callbacks

    private async void StageFinished() 
    {
        _view.SetViewAlpha(1);
        _view.TurnGeneralContainer(true);

        await Task.Delay((int)(_model.TimeBeforeBackToMaimMenuButtonAppears * 1000));
        _view.TurnBackToMainMenuButton(true,_model.BackToMainMenuButtonApperanceDuration);
    }

    #endregion

    protected override void Initialize()
    {
        _view.TurnBackToMainMenuButton(false);
        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);
    }

    public void OnBackToMainMenuButtonclick() 
    {
        _view.TurnBackToMainMenuButton(false);

        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);

        EventManager.BackedToMainMenu();
    }
}

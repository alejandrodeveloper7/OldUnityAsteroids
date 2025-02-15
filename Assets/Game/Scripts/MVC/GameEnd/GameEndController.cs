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
        EventManager.SubscribeEvent<StageFinish>(OnStageFinish);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent<StageFinish>(OnStageFinish);
    }

    #endregion

    #region Initialization

    protected override void Initialize()
    {
        _view.TurnBackToMainMenuButton(false);
        TurnView(false);
    }

    #endregion

    #region Event Callbacks

    private async void OnStageFinish(StageFinish pStageFinish)
    {
        TurnView(true);

        await Task.Delay((int)(_model.TimeBeforeBackToMaimMenuButtonAppears * 1000));

        _view.TurnBackToMainMenuButton(true, _model.BackToMainMenuButtonApperanceDuration);
    }

    #endregion

    #region Button callbacks

    public void OnBackToMainMenuButtonclick()
    {
        _view.TurnBackToMainMenuButton(false);
        TurnView(false);
        EventManager.RaiseEvent(new BackMainMenu());
    }

    #endregion

    private void TurnView(bool pState)
    {
        if (pState)
        {
            _view.SetViewAlpha(1);
            _view.TurnGeneralContainer(true);
        }
        else
        {
            _view.SetViewAlpha(0);
            _view.TurnGeneralContainer(false);
        }
    }
}

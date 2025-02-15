using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GameStatsView))]
public class GameStatsController : ControllerBase
{
    #region Fields

    [SerializeField] private GameStatsModel _model;
    private GameStatsView _view;

    [Header("Data")]
    private SO_Difficulty _difficultyData;

    [Header("Stats")]
    private int _score = 0;
    private int _comboMultiplier = 0;
    private int _movements = 0;

    #endregion

    #region Monobehaviour

    protected override void Awake()
    {
        base.Awake();
        _view = GetComponent<GameStatsView>();
    }

    private void OnEnable()
    {
        EventManager.SubscribeEvent<StageStart>(OnStageStart);
        EventManager.SubscribeEvent<StageFinish>(OnStageFinish);
        EventManager.SubscribeEvent<BackMainMenu>(OnBackMainMenu);

        EventManager.SubscribeEvent<MatchSuccess>(OnMatchSuccess);
        EventManager.SubscribeEvent<MatchFail>(OnMatchFail);

        EventManager.SubscribeEvent<GameLoad>(OnGameLoad);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent<StageStart>(OnStageStart);
        EventManager.UnsubscribeEvent<StageFinish>(OnStageFinish);
        EventManager.UnsubscribeEvent<BackMainMenu>(OnBackMainMenu);

        EventManager.UnsubscribeEvent<MatchSuccess>(OnMatchSuccess);
        EventManager.UnsubscribeEvent<MatchFail>(OnMatchFail);

        EventManager.UnsubscribeEvent<GameLoad>(OnGameLoad);
    }

    #endregion

    #region initialization

    protected override void Initialize()
    {
        TurnView(false);
        _view.SetData(_model);
    }

    #endregion

    #region Events Callback

    private void OnStageStart(StageStart pStageStart)
    {
        _difficultyData = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(diffuculty => diffuculty.Id == PersistentDataManager.DifficultyId);

        RestartStats();
        _view.TurnBackToMainMenuButton(true);

        TurnView(true);
    }
    private void OnStageFinish(StageFinish pStageFinish)
    {
        _view.TurnBackToMainMenuButton(false);
    }

    private void OnBackMainMenu(BackMainMenu pBackMainMenu)
    {
        TurnView(false);
    }

    private void OnMatchSuccess(MatchSuccess pMatchSuccess)
    {
        _movements++;
        _score += pMatchSuccess.Difficulty.CardMatchPoints;
        _comboMultiplier++;

        if (_comboMultiplier >= 2)
        {
            int comboPoints = pMatchSuccess.Difficulty.ComboPoints * (_comboMultiplier - 1);
            _score += comboPoints;
            _view.SetComboAmount(comboPoints, _comboMultiplier);
            _view.DisplayComboAnimation();
        }

        _view.SetScore(_score);
        _view.SetMovements(_movements);

        EventManager.RaiseEvent(new StatsUpdate() { Score = _score, ComboMultiplier = _comboMultiplier, Movements = _movements });
    }
    private void OnMatchFail(MatchFail pMatchFail)
    {
        _movements++;
        _comboMultiplier = 0;

        _view.SetMovements(_movements);

        EventManager.RaiseEvent(new StatsUpdate() { Score = _score, ComboMultiplier = _comboMultiplier, Movements = _movements });
    }

    private void OnGameLoad(GameLoad pGameLoad)
    {
        _difficultyData = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(diffuculty => diffuculty.Id == pGameLoad.State.DifficultyId);

        _comboMultiplier = pGameLoad.State.ComboMultiplier;
        _score = pGameLoad.State.Score;
        _movements = pGameLoad.State.Movements;

        _view.SetScore(_score);
        _view.SetMovements(_movements);
        _view.RestartComboAnimation();

        _view.TurnBackToMainMenuButton(true);

        TurnView(true);
    }

    #endregion

    #region Button callbacks

    public void OnBackToMainMenuButtonClick()
    {
        EventManager.RaiseEvent(new LeaveStage());
        TurnView(false);
    }

    #endregion

    public void RestartStats()
    {
        _comboMultiplier = 0;
        _score = 0;
        _movements = 0;

        _view.SetScore(_score);
        _view.SetMovements(_movements);
        _view.RestartComboAnimation();
    }

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

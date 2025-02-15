using System.Linq;
using UnityEngine;
using static EventManager;

[RequireComponent(typeof(GameStatsView))]
public class GameStatsController : ControllerBase
{
    #region Fields

    [SerializeField] private GameStatsModel _model;
    private GameStatsView _view;

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
        EventManager.OnStageStarted += MatchStarted;
        EventManager.OnStageFinished += OnStageFinished;
        EventManager.OnBackMainMenu += BackToMainMenu;
        EventManager.OnMatchSucess += MatchSuccess;
        EventManager.OnMatchFail += Matchfail;
    }

    private void OnDisable()
    {
        EventManager.OnStageStarted -= MatchStarted;
        EventManager.OnStageFinished -= OnStageFinished;
        EventManager.OnBackMainMenu -= BackToMainMenu;
        EventManager.OnMatchSucess -= MatchSuccess;
        EventManager.OnMatchFail -= Matchfail;
    }

    #endregion

    #region initialization

    protected override void Initialize()
    {
        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);

        _view.SetData(_model);
    }

    #endregion


    #region Events Callback

    private void MatchStarted(StageData pData)
    {
        _difficultyData = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(diffuculty => diffuculty.Id == pData.DifficultyId);
     
        RestartStats();
        _view.TurnBackToMainMenuButton(true);

        _view.SetViewAlpha(1);
        _view.TurnGeneralContainer(true);
    }

    private void OnStageFinished()
    {
        _view.TurnBackToMainMenuButton(false);
    }

    private void BackToMainMenu()
    {
        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);
    }
    #endregion

    private void MatchSuccess(SO_Difficulty pDifficulty) 
    {
        _movements++;
        _score += pDifficulty.CardMatchPoints;
        _comboMultiplier++;
        if (_comboMultiplier >= 2) 
        {
            int comboPoints = pDifficulty.ComboPoints * (_comboMultiplier - 1);
            _score += comboPoints;
            _view.SetComboAmount(comboPoints,_comboMultiplier);
            _view.DisplayComboAnimation();
        }

        _view.SetScore(_score);
        _view.SetMovements(_movements);
    }

    private void Matchfail() 
    {
        _movements++;
        _comboMultiplier = 0;
        
        _view.SetMovements(_movements);
    }

    #region Button callbacks

    public void OnBackToMainMenuButtonClick()
    {
        EventManager.StageLeaved();

        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);
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
}

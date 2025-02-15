using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MainMenuView))]
public class MainMenuController : ControllerBase
{
    #region Fields

    [SerializeField] private MainMenuModel _model;
    private MainMenuView _view;
    [Space]
    private SO_BoardSettings _boardSettings;
    private List<SO_Difficulty> _availableDifficulties;

    #endregion

    #region Monobehaviour

    protected override void Awake()
    {
        base.Awake();
        _view = GetComponent<MainMenuView>();
    }

    private void OnEnable()
    {
        EventManager.SubscribeEvent<GameStart>(OnGameStart);
        EventManager.SubscribeEvent<LeaveStage>(OnLeaveStage);
        EventManager.SubscribeEvent<BackMainMenu>(OnBackMainMenu);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent<GameStart>(OnGameStart);
        EventManager.UnsubscribeEvent<LeaveStage>(OnLeaveStage);
        EventManager.UnsubscribeEvent<BackMainMenu>(OnBackMainMenu);
    }

    #endregion

    #region Initialization

    protected override void Initialize()
    {
        GetReferences();

        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);

        _view.SetData(_model);
        _view.ConfigureSliders();
        _view.GenerateDifficultyToggles(_availableDifficulties);
    }

    private void GetReferences()
    {
        _boardSettings = ResourcesManager.Instance.GetScriptableObject<SO_BoardSettings>(ScriptableObjectKeys.BOARD_SETTINGS_KEY);
        SO_DifficultyConfiguration diffilcultyConfiguration = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY);

        _availableDifficulties = diffilcultyConfiguration.DifficultyList.Where(difficulty => difficulty.IsActive).ToList();
    }

    #endregion

    #region EventCallbacks

    private void OnGameStart(GameStart pGameStart)
    {
        TurnView(true);
    }

    private void OnLeaveStage(LeaveStage pLeaveStage)
    {
        TurnView(true);
    }

    private void OnBackMainMenu(BackMainMenu pBackMainMenu)
    {
        TurnView(true);
    }

    #endregion

    #region Button Callbacks

    public void OnPlayButtonClick()
    {
        PersistentDataManager.ColumnsAmount = _view.ColumnsAmount;
        PersistentDataManager.RowsAmount = _view.RowsAmount;
        PersistentDataManager.DifficultyId = _availableDifficulties[_view.DifficultyIndexSelected].Id;

        TurnView(false);

        EventManager.RaiseEvent(new StageStart());
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


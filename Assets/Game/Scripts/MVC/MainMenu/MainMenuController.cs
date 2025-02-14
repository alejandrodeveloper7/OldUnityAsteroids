using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MainMenuView))]
public class MainMenuController : ControllerBase
{
    #region Fields

    [SerializeField] private MainMenuModel _model;
    private MainMenuView _view;

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
        EventManager.OnGameStarted += StartGame;
    }

    private void OnDisable()
    {
        EventManager.OnGameStarted -= StartGame;

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

    private void StartGame()
    {
        _view.TurnGeneralContainer(true);
        _view.SetViewAlpha(1);
    }

    #endregion

    #region Button Callbacks

    public void OnPlayButtonClick() 
    {
        PersistentDataManager.ColumnsAmount = _view.ColumnsAmount;
        PersistentDataManager.RowsAmount = _view.ColumnsAmount;

        PersistentDataManager.DifficultyId = _availableDifficulties[_view.DifficultyIndexSelected].Id;

        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);

        EventManager.MatchStarted(new EventManager.MatchData()
        {
            ColumnsAmount = PersistentDataManager.ColumnsAmount,
            RowsAmount = PersistentDataManager.RowsAmount,
            DifficultyId = PersistentDataManager.DifficultyId,
        });
    }

    #endregion
}

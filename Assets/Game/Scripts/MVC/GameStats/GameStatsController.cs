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

    #endregion

    #region Monobehaviour
    
    protected override void Awake()
    {
        base.Awake();
        _view = GetComponent<GameStatsView>();
    }

    private void OnEnable()
    {
        EventManager.OnMatchStarted += MatchStarted;
    }

    private void OnDisable()
    {
        EventManager.OnMatchStarted -= MatchStarted;
    }

    #endregion

    #region initialization

    protected override void Initialize()
    {
        _view.SetViewAlpha(0);
        _view.TurnGeneralContainer(false);
    }

    #endregion


    #region Events Callback
    
    private void MatchStarted(MatchData pData) 
    {
        _difficultyData = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(diffuculty => diffuculty.Id == pData.DifficultyId);
        
        _view.SetViewAlpha(1);
        _view.TurnGeneralContainer(true);
    }
    
    #endregion
}

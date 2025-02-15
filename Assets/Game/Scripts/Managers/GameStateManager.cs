using System;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private GameState _currentGameState = new GameState();

    [Header("Data")]
    private SO_GeneralSettings _generalSettings;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.SubscribeEvent<SaveGame>(OnSaveData);
        EventManager.SubscribeEvent<GameLoad>(OnGameLoad);
        EventManager.SubscribeEvent<CleanSaveData>(OnCleanSaveData);

        EventManager.SubscribeEvent<StageStart>(OnStageStart);
        EventManager.SubscribeEvent<StageFinish>(OnStageFinish);
        EventManager.SubscribeEvent<BoardGeneration>(OnBoardGeneration);

        EventManager.SubscribeEvent<MatchStart>(OnMatchStart);
        EventManager.SubscribeEvent<MatchSuccess>(OnMatchSuccess);
        EventManager.SubscribeEvent<MatchFail>(OnMatchFail);

        EventManager.SubscribeEvent<StatsUpdate>(OnStatsUpdate);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent<SaveGame>(OnSaveData);
        EventManager.UnsubscribeEvent<GameLoad>(OnGameLoad);
        EventManager.UnsubscribeEvent<CleanSaveData>(OnCleanSaveData);

        EventManager.UnsubscribeEvent<StageStart>(OnStageStart);
        EventManager.UnsubscribeEvent<StageFinish>(OnStageFinish);
        EventManager.UnsubscribeEvent<BoardGeneration>(OnBoardGeneration);

        EventManager.UnsubscribeEvent<MatchStart>(OnMatchStart);
        EventManager.UnsubscribeEvent<MatchSuccess>(OnMatchSuccess);
        EventManager.UnsubscribeEvent<MatchFail>(OnMatchFail);

        EventManager.UnsubscribeEvent<StatsUpdate>(OnStatsUpdate);
    }

    #endregion

    #region Event Callbacks

    private void OnSaveData(SaveGame pSaveData)
    {
        if (_generalSettings.SaveGameState)
            SaveDataManager.GenerateFile(_currentGameState, _generalSettings.FileName);
    }
    private void OnGameLoad(GameLoad pGameLoad)
    {
        _currentGameState = pGameLoad.State;
    }
    private void OnCleanSaveData(CleanSaveData pCleanSaveData)
    {
        CleanData();
    }

    private void OnStageStart(StageStart pStageStart)
    {
        _currentGameState.DifficultyId = PersistentDataManager.DifficultyId;
        _currentGameState.BoardRows = Mathf.RoundToInt(PersistentDataManager.RowsAmount);
        _currentGameState.BoardColumns = Mathf.RoundToInt(PersistentDataManager.ColumnsAmount);
    }
    private void OnStageFinish(StageFinish pStageFinish)
    {
        CleanData();
    }
    private void OnBoardGeneration(BoardGeneration pBoardGeneration)
    {
        SaveBoard(pBoardGeneration.Board);
    }

    private void OnMatchStart(MatchStart pMatchStart)
    {
        SaveBoard(pMatchStart.Board);
    }
    private void OnMatchSuccess(MatchSuccess pMatchSuccess)
    {
        SaveBoard(pMatchSuccess.Board);
    }
    private void OnMatchFail(MatchFail pMatchFail)
    {
        SaveBoard(pMatchFail.Board);
    }

    private void OnStatsUpdate(StatsUpdate pStatsUpdate)
    {
        _currentGameState.Score = pStatsUpdate.Score;
        _currentGameState.ComboMultiplier = pStatsUpdate.ComboMultiplier;
        _currentGameState.Movements = pStatsUpdate.Movements;
    }

    #endregion

    private void GetReferences()
    {
        _generalSettings = ResourcesManager.Instance.GetScriptableObject<SO_GeneralSettings>(ScriptableObjectKeys.GENERAL_SETTINGS_KEY);
    }

    private void SaveBoard(List<CardController> pBoard)
    {
        _currentGameState.Cards = new List<CardState>();

        foreach (CardController card in pBoard)
        {
            CardState newCardState = new CardState()
            {
                CardId = card.CardData.Id,
                IsCleaned = card.IsCleaned,
                IsSelected = card.IsSelected
            };
            _currentGameState.Cards.Add(newCardState);
        }
    }

    private void CleanData()
    {
        _currentGameState = new GameState();
        SaveDataManager.DeleteFile(_generalSettings.FileName);
    }
}

#region Models

[Serializable]
public class GameState
{
    public int DifficultyId;
    public int BoardRows;
    public int BoardColumns;
    public List<CardState> Cards;
    public int ComboMultiplier;
    public int Score;
    public int Movements;
}

[Serializable]
public class CardState
{
    public int CardId;
    public bool IsCleaned;
    public bool IsSelected;
}

#endregion

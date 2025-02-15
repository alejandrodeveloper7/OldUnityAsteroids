using System;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class GameStateManager : MonoBehaviour
{
    private GameState _currentGameState = new GameState();

    private SO_GeneralSettings _generalSettings;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        _generalSettings = ResourcesManager.Instance.GetScriptableObject<SO_GeneralSettings>(ScriptableObjectKeys.GENERAL_SETTINGS_KEY);
    }


    private void OnEnable()
    {
        EventManager.OnGameStateLoaded += OnGameStateLoaded;
        EventManager.OnCleanSavedData += OnCleanSaveData;
        EventManager.OnStageStarted += OnStageStarted;
        EventManager.OnBoardGenerated += OnBoardGenerated;
        EventManager.OnStageFinished += OnStageFinished;

        EventManager.OnStartMatch += OnMatchStarted;
        EventManager.OnMatchSucess += OnMatchSuccesed;
        EventManager.OnMatchFail += OnMatchFailed;

        EventManager.OnStatsUpdate += OnStatsUpdated;

        EventManager.OnSaveData += OnSaveData;
    }

    private void OnDisable()
    {
        EventManager.OnGameStateLoaded -= OnGameStateLoaded;
        EventManager.OnCleanSavedData -= OnCleanSaveData;
        EventManager.OnStageStarted -= OnStageStarted;
        EventManager.OnBoardGenerated -= OnBoardGenerated;
        EventManager.OnStageFinished -= OnStageFinished;

        EventManager.OnStartMatch -= OnMatchStarted;
        EventManager.OnMatchSucess -= OnMatchSuccesed;
        EventManager.OnMatchFail -= OnMatchFailed;

        EventManager.OnStatsUpdate -= OnStatsUpdated;

        EventManager.OnSaveData -= OnSaveData;

    }

    private void OnGameStateLoaded(GameState pData)
    {
        _currentGameState = pData;
    }

    private void OnCleanSaveData()
    {
        CleanData();
    }

    private void OnStageFinished()
    {
        CleanData();

    }

    private void OnStageStarted(StageData pData)
    {
        _currentGameState.DifficultyId = pData.DifficultyId;
        _currentGameState.BoardRows = Mathf.RoundToInt(pData.RowsAmount);
        _currentGameState.BoardColumns = Mathf.RoundToInt(pData.ColumnsAmount);
    }

    private void OnBoardGenerated(List<CardController> pBoard)
    {
        SaveBoard(pBoard);
    }
    private void OnMatchStarted(List<CardController> pBoard)
    {
        SaveBoard(pBoard);
    }
    private void OnMatchSuccesed(SO_Difficulty pDifficulty, List<CardController> pBoard)
    {
        SaveBoard(pBoard);
    }
    private void OnMatchFailed(List<CardController> pBoard)
    {
        SaveBoard(pBoard);
    }

    private void OnStatsUpdated(int pScore, int pComboMultiplier, int pMovement)
    {
        _currentGameState.Score = pScore;
        _currentGameState.ComboMultiplier = pComboMultiplier;
        _currentGameState.Movements = pMovement;
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

    private void OnSaveData()
    {
        if (_generalSettings.SaveGameState)
            SaveDataManager.SaveToJson(_currentGameState, _generalSettings.FileName);
    }

}

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

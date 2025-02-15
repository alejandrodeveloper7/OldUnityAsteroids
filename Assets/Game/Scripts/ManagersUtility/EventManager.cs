using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //This class should be automated with a dictionary and Generics to be scalable, but with this few events, this implementation works fine for the test and is quicker 

    public delegate void StartGame();
    public static event StartGame OnGameStarted = () => { };
    public static void GameStarted()
    {
        OnGameStarted.Invoke();
    }


    public delegate void GameStateLoad(GameState pData);
    public static event GameStateLoad OnGameStateLoaded = (data) => { };
    public static void GameStateLoaded(GameState pData)
    {
        OnGameStateLoaded.Invoke(pData);
    }


    public delegate void StartStage(StageData pData);
    public static event StartStage OnStageStarted = (data) => { };
    public class StageData
    {
        public float ColumnsAmount;
        public float RowsAmount;
        public int DifficultyId;
    }
    public static void StageStarted(StageData pData)
    {
        Debug.Log("--- Stage Started");
        OnStageStarted.Invoke(pData);
    }



    public delegate void StageFinish();
    public static event StageFinish OnStageFinished = () => { };
    public static void StageFinished()
    {
        Debug.Log("--- Stage Finished");
        OnStageFinished.Invoke();
    }



    public delegate void LeaveStage();
    public static event LeaveStage OnStageLeaved = () => { };
    public static void StageLeaved()
    {
        Debug.Log("--- Stage Leaved");
        OnStageLeaved.Invoke();
    }


    public delegate void BackMainMenu();
    public static event BackMainMenu OnBackMainMenu = () => { };
    public static void BackedToMainMenu()
    {
        OnBackMainMenu.Invoke();
    }


    public delegate void RotateCard(CardController pCard);
    public static event RotateCard OnCardRotated = (card) => { };
    public static void CardRotated(CardController pCard)
    {
        OnCardRotated.Invoke(pCard);
    }




    public delegate void StartMatch(List<CardController> pBoard);
    public static event StartMatch OnStartMatch = (board) => { };
    public static void MatchStarted(List<CardController> pBoard)
    {
        OnStartMatch.Invoke(pBoard);
    }

    public delegate void MatchSucess(SO_Difficulty pDifficulty, List<CardController> pBoard);
    public static event MatchSucess OnMatchSucess = (difficulty, board) => { };
    public static void MatchSucessed(SO_Difficulty pDifficulty, List<CardController> pBoard)
    {
        OnMatchSucess.Invoke(pDifficulty, pBoard);
    }

    public delegate void MatchFail(List<CardController> pBoard);
    public static event MatchFail OnMatchFail = (board) => { };
    public static void MatchFailed(List<CardController> pBoard)
    {
        OnMatchFail.Invoke(pBoard);
    }



    public delegate void SoundGeneration(SO_Sound pSound);
    public static event SoundGeneration OnGenerateSound = (sound) => { };
    public static void GenerateSound(SO_Sound pSound)
    {
        OnGenerateSound.Invoke(pSound);
    }

    public delegate void DataSave();
    public static event DataSave OnSaveData = () => { };
    public static void SaveData()
    {
        OnSaveData.Invoke();
    }


    public delegate void BoardGeneration(List<CardController> pBoard);
    public static event BoardGeneration OnBoardGenerated = (board) => { };
    public static void BoardGenerated(List<CardController> pBoard)
    {
        OnBoardGenerated.Invoke(pBoard);
    }

    public delegate void StatsUpdate(int pScore, int pComboMultiplier, int pMovements);
    public static event StatsUpdate OnStatsUpdate = (score, comboMultiplier, movements) => { };
    public static void StatsUpdated(int pScore, int pComboMultiplier, int pMovement)
    {
        OnStatsUpdate.Invoke(pScore, pComboMultiplier, pMovement);
    }

    public delegate void CleanSavedData();
    public static event CleanSavedData OnCleanSavedData = () => { };
    public static void CleanSaveData()
    {
        OnCleanSavedData.Invoke();
    }
}

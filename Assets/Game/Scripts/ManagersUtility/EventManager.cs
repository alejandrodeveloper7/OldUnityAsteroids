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



    public delegate void LoadGameState();
    public static event LoadGameState OnGameStateLoaded = () => { };
    public static void GameStateloaded()
    {
        OnGameStateLoaded.Invoke();
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


    public delegate void RotateCard (CardController pCard);
    public static event RotateCard OnCardRotated = (card) => { };
    public static void CardRotated(CardController pCard)
    {
        OnCardRotated.Invoke(pCard);
    }




    public delegate void StartMatch();
    public static event StartMatch OnStartMatch = () => { };
    public static void MatchStarted()
    {
        OnStartMatch.Invoke();
    }

    public delegate void MatchSucess();
    public static event MatchSucess OnMatchSucess = () => { };
    public static void MatchSucessed()
    {
        OnMatchSucess.Invoke();
    }

    public delegate void MatchFail();
    public static event MatchFail OnMatchFail = () => { };
    public static void MatchFailed()
    {
        OnMatchFail.Invoke();
    }



    public delegate void SoundGeneration(SO_Sound pSound);
    public static event SoundGeneration OnGenerateSound = (sound) => { };
    public static void GenerateSound(SO_Sound pSound)
    {
        OnGenerateSound.Invoke(pSound);
    }
}

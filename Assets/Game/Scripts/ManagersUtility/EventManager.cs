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



    public delegate void StartMatch(MatchData data);
    public static event StartMatch OnMatchStarted = (data) => { };
    public class MatchData
    {
        public float ColumnsAmount;
        public float RowsAmount;
        public int DifficultyId;
    }
    public static void MatchStarted(MatchData pData)
    {
        Debug.Log("--- Match Started");
        OnMatchStarted.Invoke(pData);
    }



    public delegate void MatchFinish();
    public static event MatchFinish OnMatchFinished = () => { };
    public static void MatchFinished()
    {
        Debug.Log("--- Match Finished");
        OnMatchFinished.Invoke();
    }



    public delegate void LeaveMatch();
    public static event LeaveMatch OnMatchLeaved = () => { };
    public static void MatchLeaved()
    {
        Debug.Log("--- Match Leaved");
        OnMatchLeaved.Invoke();
    }
}

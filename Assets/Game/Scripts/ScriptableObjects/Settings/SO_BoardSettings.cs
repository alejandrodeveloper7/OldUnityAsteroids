using UnityEngine;

[CreateAssetMenu(fileName = "NewBoardSettings", menuName = "ScriptableObjects/Settings/BoardSettings")]
public class SO_BoardSettings : ScriptableObject
{
    [Header("CardsPool")]
    public GameObject CardPrefab;
    [Space]
    public string ParentName;
    public Vector3 ParentPosition;
    [Space]
    public int PoolInitialSize;
    public int PoolIncrement;

    [Header("Values")]
    public float SaveGameDelayAfterNewBoardGeneration;

    [Header("Sound")]
    public SO_Sound SoundOnMatchSuccess;
    public SO_Sound SoundOnMatchFailed;
    [Space]
    public SO_Sound SoundOnStageComplete;
}

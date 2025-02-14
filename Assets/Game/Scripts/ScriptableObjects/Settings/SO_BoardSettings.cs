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
}

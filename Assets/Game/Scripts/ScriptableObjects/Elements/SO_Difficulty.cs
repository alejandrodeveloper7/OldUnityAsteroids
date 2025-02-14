using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficulty", menuName = "ScriptableObjects/Elements/Difficulty")]
public class SO_Difficulty : ScriptableObject
{
    [Header("General")]
    public int Id;
    public string Name;
    [Space]
    public bool IsActive;

    [Header("Configuration")]
    public float InitialCardsShowTime;
    [Space]
    public int CardMatchPoints;
    public int ComboPoints;
}

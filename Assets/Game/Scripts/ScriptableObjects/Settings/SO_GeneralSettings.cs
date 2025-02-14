using UnityEngine;

[CreateAssetMenu(fileName = "NewGeneralSettings", menuName = "ScriptableObjects/Settings/GeneralSettings")]
public class SO_GeneralSettings : ScriptableObject
{
    [Header("General")]
    public int TargetFrameRate;
 
    [Header("Game State")]
    public bool UseSavedGameState; 
}

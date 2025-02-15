using UnityEngine;

[CreateAssetMenu(fileName = "GameEndModel", menuName = "ScriptableObjects/MVCModels/GameEndModel")]
public class GameEndModel : ModelBase
{
    [Header("Times")]
    public float TimeBeforeBackToMaimMenuButtonAppears;
    public float BackToMainMenuButtonApperanceDuration;
}

using UnityEngine;

[CreateAssetMenu(fileName = "GameStatsModel", menuName = "ScriptableObjects/MVCModels/GameStatsModel")]
public class GameStatsModel : ModelBase
{
    [Header("Times")]
    public float CombosScaleDuration;
    public float CombosDisplayDuration;
}

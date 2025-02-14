using UnityEngine;

[CreateAssetMenu(fileName = "MainMenuModel", menuName = "ScriptableObjects/MVCModels/MainMenuModel")]
public class MainMenuModel : ScriptableObject
{
    [Header("Sliders")]
    public int MinColumnsValue;
    public int MaxColumnsValue;
    public int ColumnsMinSelectebleValue;
    [Space]
    public int MinRowsValue;
    public int MaxRowsValue;
    public int RowsMinSelectebleValue;

    [Header("Difficulty Toggles")]
    public GameObject DifficultyTogglePrefab;
}

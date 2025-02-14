using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "ScriptableObjects/Elements/Card")]
public class SO_Card : ScriptableObject
{
    [Header("Configuration")]
    public int Id;
    public bool IsActive;
    [Space]
    public Sprite Image;
}

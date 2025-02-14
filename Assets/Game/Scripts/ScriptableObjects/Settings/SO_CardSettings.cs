using UnityEngine;

[CreateAssetMenu(fileName = "NewCardSettings", menuName = "ScriptableObjects/Settings/CardSettings")]
public class SO_CardSettings : ScriptableObject
{
    [Header("Settings")]
    public float RotationDuration;
    public float DissaparitionDuration;
    public float TimeBeforeDissapear;
    [Space]
    public float HoverDuration;
    public Vector3 HoverScale;
}

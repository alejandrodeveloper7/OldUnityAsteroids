using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundSettings", menuName = "ScriptableObjects/Settings/SoundSettings")]
public class SO_SoundSettings : ScriptableObject
{
    [Header("AudioSource Pool")]
    public int PoolInitialSize;
    public int PoolIncrement;
}

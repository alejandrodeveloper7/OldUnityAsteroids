using UnityEngine;

[CreateAssetMenu(fileName = "NewSound", menuName = "ScriptableObjects/Elements/Sound")]
public class SO_Sound : ScriptableObject
{
    [Header("Clip")]
    public AudioClip Clip;
    [Range(0, 1f)] public float Volume;

    [Header("Configuration")]
    public bool PlayOnAwake;
    public bool Loop;


    public void ApplyConfig(AudioSource pAudioSource)
    {
        pAudioSource.clip = Clip;
        pAudioSource.volume = Volume;
        pAudioSource.loop = Loop;
        pAudioSource.playOnAwake = PlayOnAwake;
    }
}

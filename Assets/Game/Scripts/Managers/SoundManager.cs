using System.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields

    private SO_SoundSettings _soundSettings;
    private SO_MusicConfiguration _musicConfiguration;
    [Space]
    private int _currentMusicIndex = -1;
    [Space]
    private AudioSource _musicSource;
    private AudioSourcePool _audioSourcesPool;
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
        Create2DAudioSourcePool();

        PlayMusicLoop();
    }

    private void OnEnable()
    {
        EventManager.OnGenerateSound += GenerateSound;
    }

    private void OnDisable()
    {
        EventManager.OnGenerateSound -= GenerateSound;
    }

    #endregion

    #region Events Callbacks


    private void GenerateSound(SO_Sound pData)
    {
        Create2DSound(pData);
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _soundSettings = ResourcesManager.Instance.GetScriptableObject<SO_SoundSettings>(ScriptableObjectKeys.SOUND_SETTINGS_KEY);
        _musicConfiguration = ResourcesManager.Instance.GetScriptableObject<SO_MusicConfiguration>(ScriptableObjectKeys.MUSIC_CONFIGURATION_KEY);

        _musicSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
    }

    private void Create2DAudioSourcePool()
    {
        _audioSourcesPool = new AudioSourcePool(_soundSettings.PoolInitialSize, _soundSettings.PoolIncrement);
    }

    #endregion

    #region Music

    private async void PlayMusicLoop()
    {
        while (this)
        {
            PlayRandomTrack();
            await WaitForMusicEnd();
        }
    }

    private void PlayRandomTrack()
    {
        int newTrackIndex;
        newTrackIndex = Random.Range(0, _musicConfiguration.MusicList.Count);

        _currentMusicIndex = newTrackIndex;
        SO_Sound currentMusicData = _musicConfiguration.MusicList[_currentMusicIndex];
        currentMusicData.ApplyConfig(_musicSource);

        _musicSource.Play();
    }

    private async Task WaitForMusicEnd()
    {
        while (_musicSource && _musicSource.isPlaying)
            await Task.Yield();
    }

    #endregion

    #region Sounds

    private void Create2DSound(SO_Sound pData)
    {
        AudioSource audioSource = _audioSourcesPool.GetInstance();
        pData.ApplyConfig(audioSource);
        audioSource.Play();
    }

    #endregion
}

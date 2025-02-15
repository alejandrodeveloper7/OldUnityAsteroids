using System.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Fields

    [Header("Data")]
    private SO_SoundSettings _soundSettings;
    private SO_MusicConfiguration _musicConfiguration;

    [Header("Music")]
    private AudioSource _musicSource;
    private int _currentMusicIndex = -1;

    [Header("Sound")]
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
        EventManager.SubscribeEvent<Generate2DSound>(OnGenerate2DSound);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent<Generate2DSound>(OnGenerate2DSound);
    }

    #endregion

    #region Events Callbacks

    private void OnGenerate2DSound(Generate2DSound pGenerate2DSound)
    {
        Create2DSound(pGenerate2DSound.Sound);
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
        if (_musicConfiguration.MusicList.Count == 0)
            return;

        while (this)
        {
            PlayRandomTrack();
            await WaitForMusicEnd();
        }
    }

    private void PlayRandomTrack()
    {
        int newTrackIndex = 0;

        if (_musicConfiguration.MusicList.Count > 1)
        {
            do
                newTrackIndex = Random.Range(0, _musicConfiguration.MusicList.Count);
            while (newTrackIndex == _currentMusicIndex);
        }

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

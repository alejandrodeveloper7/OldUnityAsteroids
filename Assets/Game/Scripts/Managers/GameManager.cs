using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SO_GeneralSettings _generalSettings;

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
        ScreenManager.FixFrameRate(_generalSettings.TargetFrameRate);
    }

    private void Start()
    {
        if (_generalSettings.UseSavedGameState)
            TryLoadSavedGameState();
        else
            StartGame();
    }

    #endregion

    private void GetReferences()
    {
        _generalSettings = ResourcesManager.Instance.GetScriptableObject<SO_GeneralSettings>(ScriptableObjectKeys.GENERAL_SETTINGS_KEY);
    }

    #region GameFlow

    private async void StartGame()
    {
        await Task.Yield();
        EventManager.RaiseEvent(new GameStart());
    }

    private void TryLoadSavedGameState()
    {
        if (SaveDataManager.FileExist(_generalSettings.FileName))
            EventManager.RaiseEvent(new GameLoad() { State = SaveDataManager.LoadFile<GameState>(_generalSettings.FileName) });
        else
            StartGame();
    }

    #endregion
}


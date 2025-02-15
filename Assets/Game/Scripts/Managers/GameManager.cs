using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SO_GeneralSettings _generalSettings;

    private void Awake()
    {
        GetReferences();
        ScreenManager.FixFrameRate(_generalSettings.TargetFrameRate);
    }

    private void GetReferences()
    {
        _generalSettings = ResourcesManager.Instance.GetScriptableObject<SO_GeneralSettings>(ScriptableObjectKeys.GENERAL_SETTINGS_KEY);
    }

    private void Start()
    {
        if (_generalSettings.UseSavedGameState)
            LoadSavedGameState();
        else         
            StartGame();        
    }

    private async void StartGame()
    {
        await Task.Yield();
        EventManager.GameStarted();
    }

    private void LoadSavedGameState()
    {
        if (SaveDataManager.FileExist(_generalSettings.FileName))
            EventManager.GameStateLoaded(SaveDataManager.LoadFromJson<GameState>(_generalSettings.FileName));
        else        
            StartGame();       
    }
}

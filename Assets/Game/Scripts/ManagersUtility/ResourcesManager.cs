using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    public static ResourcesManager Instance { get; } = new ResourcesManager();
    private Dictionary<string, ScriptableObject> _scriptableObjects = new Dictionary<string, ScriptableObject>();

    #region Initialization

    private ResourcesManager()
    {
        Initialize();
    }

    private void Initialize()
    {
        LoadScriptablesFromFolder("Settings");
        LoadScriptablesFromFolder("Configurations");
    }

    #endregion

    #region ScriptableObject managements

    private void LoadScriptablesFromFolder(string pFolderName)
    {
        ScriptableObject[] loadedObjects = Resources.LoadAll<ScriptableObject>(pFolderName);
        foreach (ScriptableObject obj in loadedObjects)
            _scriptableObjects[obj.name] = obj;
    }

    public T GetScriptableObject<T>(string pName) where T : ScriptableObject
    {
        if (_scriptableObjects.TryGetValue(pName, out ScriptableObject obj))
            return obj as T;

        Debug.LogError(string.Format("{0} scriptableobject not found", pName));
        return null;
    }

    #endregion
}

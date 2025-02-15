using System.IO;
using UnityEngine;

public class SaveDataManager
{
    public static void GenerateFile<T>(T pData, string pFileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, pFileName + ".json");
        string json = JsonUtility.ToJson(pData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("File Saved");
    }

    public static T LoadFile<T>(string pFileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, pFileName + ".json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }

        Debug.Log("File not found");
        return default;
    }

    public static bool FileExist(string pFileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, pFileName + ".json");
        if (File.Exists(filePath))
            return true;
        else
            return false;
    }

    public static void DeleteFile(string pFileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, pFileName + ".json");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("File Erased");
        }
        else        
            Debug.Log("File not found");        
    }
}

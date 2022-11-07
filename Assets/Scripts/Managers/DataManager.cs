using NaughtyAttributes;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private void Awake()
    {
        if(instance == null) instance = this;
    }

    [Header ("Data")]
    public GameData gameData;
    public PlayerData playerData;
    
    [Header ("Save Path")]
    [SerializeField] private bool _isPersistance;
    private const string _SAVE_FILE = "SaveData\\SaveFile.json";

    public void SavePlayerData()
    {
        string data = JsonUtility.ToJson(playerData);
        string path = GetSavePath();
        File.WriteAllText(path, data);
    }

    public void LoadPlayerData()
    {
        string path = GetSavePath();
        string jsonText = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(jsonText);
    }

    private string GetSavePath()
    {
        return (!_isPersistance) ? Application.dataPath + Path.AltDirectorySeparatorChar + _SAVE_FILE 
                                : Application.persistentDataPath + Path.AltDirectorySeparatorChar + _SAVE_FILE;
    }

    [Button]
    public void ClearPlayerData()
    {
        playerData = new PlayerData();
        SavePlayerData();
    }
}

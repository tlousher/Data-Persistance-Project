using System.IO;
using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private const string FileName = "/savefile.json";
    private static string Path => Application.persistentDataPath + FileName;

    private GameData _data;
    private static DataManager _instance;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    [Serializable]
    public class GameData
    {
        public string name;
        public int score;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                SaveData();
            }
        }

        public int Score
        {
            get => score;
            set
            {
                score = value;
                SaveData();
            }
        }
    }

    internal static GameData Data
    {
        get => _instance._data;
        set
        {
            _instance._data = value;
            SaveData();
        }
    }

    private static void SaveData()
    {
        var json = JsonUtility.ToJson(Data);
        File.WriteAllText(Path, json);
    }

    private static void LoadData()
    {
        if (!File.Exists(Path))
        {
            Data = new GameData();
            return;
        }
        
        var json = File.ReadAllText(Path);
        Data = JsonUtility.FromJson<GameData>(json);
    }
}

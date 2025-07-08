using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
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
        _data = new GameData();
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    [Serializable]
    public class GameData
    {
        public string jsonName;
        public int[] jsonHighScore;
        public string[] jsonHighScoreNames;
        
        internal Dictionary<string, int> HighScoreDict;
        
        public GameData()
        {
            jsonName = "Player";
            jsonHighScore = null;
            jsonHighScoreNames = null;
            HighScoreDict = new Dictionary<string, int>();
        }
        
        public GameData(GameData data)
        {
            jsonName = data.jsonName;
            jsonHighScore = data.jsonHighScore;
            jsonHighScoreNames = data.jsonHighScoreNames;
            
            PoolDictionary();
        }

        public string Name
        {
            get => jsonName;
            set
            {
                jsonName = value;
                if (!HighScoreDict.TryAdd(jsonName, 0))
                    HighScoreDict[jsonName] = 0;
                else
                {
                    SaveData();
                }
            }
        }

        public int HighScore
        {
            get => HighScoreDict.Values.Prepend(0).Max();
            set
            {
                if (!HasName) return;
                
                // if the new score is higher than the old one, we update the high score
                if (!HighScoreDict.TryAdd(jsonName, value))
                    HighScoreDict[jsonName] = Math.Max(value, HighScoreDict[jsonName]);

                UpdateArrays();
                SaveData();
            }
        }

        internal string GetHighScoreName
        {
            get
            {
                var highestScore = HighScoreDict.Values.Prepend(0).Max();
                return HighScoreDict.First(x => x.Value == highestScore).Key;
            }
        }

        internal void PoolDictionary()
        {
            HighScoreDict = new Dictionary<string, int>();
            foreach (var key in jsonHighScoreNames)
            {
                HighScoreDict.Add(key, jsonHighScore[Array.IndexOf(jsonHighScoreNames, key)]);
            }
        }

        internal void UpdateArrays()
        {
            jsonHighScoreNames = new string[HighScoreDict.Count];
            jsonHighScore = new int[HighScoreDict.Count];
            var i = 0;
            foreach (var key in HighScoreDict.Keys)
            {
                jsonHighScoreNames[i] = key;
                jsonHighScore[i] = HighScoreDict[key];
                i++;
            }
        }
        
        internal bool HasName => jsonName != "Player";
    }

    internal static GameData Data
    {
        get => _instance._data;
        set => _instance._data = value;
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
        Data = new GameData(JsonUtility.FromJson<GameData>(json));
    }
}

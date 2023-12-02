using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static string saveFile = "";
    public static GameController Instance;
    private const int MAX_RANKED = 5;
    
    private List<PlayerScore> ranking = new List<PlayerScore>(MAX_RANKED);
    public string playerName { get; set; } // current player name

    // Start is called before the first frame update
    private void Awake()
    {
        // Try to ensure a singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFile = Application.persistentDataPath + Path.DirectorySeparatorChar + "ranking.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int score)
    {
        ranking.Add(new PlayerScore(playerName, score));
        ranking.Sort((x,y) => y.Score.CompareTo(x.Score));
        ranking = ranking.Take(ranking.Count > MAX_RANKED ? MAX_RANKED : ranking.Count).ToList();
    }

    public void SaveRanking()
    {
        SaveData data = new SaveData();
        int rankedPlayers = Math.Min(ranking.Count, MAX_RANKED);
        data.scores = ranking.Take(rankedPlayers).ToList();

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFile, json);
    }

    public List<PlayerScore> loadRanking()
    {
        if (File.Exists(saveFile))
        {
            string json = File.ReadAllText(saveFile);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            ranking.Clear();
            int rankedPlayers = Math.Min(data.scores.Count, MAX_RANKED);

            ranking = data.scores.Take(rankedPlayers).ToList();
            ranking.Sort((x,y) => x.Score.CompareTo(x.Score));
        }

        return ranking;
    }
     

    [Serializable]
    public class PlayerScore
    {
        public PlayerScore(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public string Name;
        public int Score;
    }
    
    [Serializable]
    class SaveData
    {
        public List<PlayerScore> scores;
    }

}

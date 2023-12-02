using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{

    public TMP_InputField playerNameField;
    public TMP_Text rankingText;

    private void Start()
    {
        // Try to load ranking
        UpdateRanking();
    }

    // Update the ranking
    void UpdateRanking()
    {
        if (GameController.Instance == null)
        {
            return; 
        }
        List<GameController.PlayerScore> ranking = GameController.Instance.loadRanking();

        string rankingStr = "Top 5 Players \n";
        foreach (GameController.PlayerScore score in ranking)
        {
            rankingStr += score.Name + " \t " + score.Score + "\n";
        }

        rankingText.text = rankingStr;
    }
    
    // Set the player name to MainManager
    void SetPlayerName()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.playerName = playerNameField.text;
        }

    }

    bool IsValidPlayerName()
    {
        return playerNameField.text.Length > 3;
    }

    public void StartGame()
    {
        if (!IsValidPlayerName())
        {
            // TODO: Set a red border on input file
            return;
        }

        if (GameController.Instance == null)
        {
            return;
        }
        
        SetPlayerName();
        SceneManager.LoadScene("Scenes/main");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public enum Score
    {
        AiScore, PlayerScore
    }

    [SerializeField] private UIManager uiManager;
    public Text AiScoreTxt, PlayerScoreTxt;
    private int aiScore, playerScore;
    [SerializeField] private int maxScore;
    
    #region Scores

    private int AiScore
    {
        get { return aiScore; }
        set
        {
            aiScore = value;
            if (value == maxScore)
                uiManager.ShowRestartCanvas(true);
        }
    }

    private int PlayerScore
    {
        get { return playerScore; }
        set
        {
            playerScore = value;
            if (value == maxScore)
                uiManager.ShowRestartCanvas(false);
        }
    }

    #endregion
        
    public void Increment(Score whichScore)
    {
        if (whichScore == Score.AiScore)
            AiScoreTxt.text = (++AiScore).ToString();
        else
            PlayerScoreTxt.text = (++PlayerScore).ToString();
    }
    
    public void ResetScores()
    {
        AiScore = PlayerScore = 0;
        AiScoreTxt.text = PlayerScoreTxt.text = "0";
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject resultCanvas;
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private Text resultText;
    [SerializeField] private Button restartButton;
    
    [Header("Other")]
    public AudioManager audioManager;

    public ScoreManager scoreScript;

    public PuckScript puckScript;
    public PlayerMovement playerMovement;
    public AIController aiScript;

    private void OnEnable()
    {
        AddButtonListeners();
    }

    private void AddButtonListeners()
    {
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        RemoveButtonListeners();
    }

    private void RemoveButtonListeners()
    {
        restartButton.onClick.RemoveListener(RestartGame);
    }

    public void ShowRestartCanvas(bool didAiWin)
    {
        Time.timeScale = 0f;

        ShowResultPanel(true);

        if (didAiWin)
        {
            audioManager.PlayLostGame();
            resultText.text = "You Lost!!";
        }
        else
        {
            audioManager.PlayWonGame();
            resultText.text = "You Won!!";
        }
    }

    private void ShowResultPanel(bool value)
    {
        scoreCanvas.SetActive(!value);
        resultCanvas.SetActive(value);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        
        ShowResultPanel(false);
        
        scoreScript.ResetScores();
        puckScript.CenterPuck();
        playerMovement.ResetPosition();
        aiScript.ResetPosition();
    }
}

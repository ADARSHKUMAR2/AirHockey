using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    public static GameSetup gameSetup;

    // public Transform[] spawnPoints;
    public Transform[] spawnPointsTeamOne;
    public Transform[] spawnPointsTeamTwo;
    public int nextTeam;
    public Text healthDisplay;

    private void OnEnable()
    {
        if (gameSetup == null)
            gameSetup = this;
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    private IEnumerator DisconnectAndLoad()
    {
        // PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        // while (PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }

    public void UpdateTeam()
    {
        if (nextTeam == 1)
            nextTeam = 2;
        else
            nextTeam = 1;
    }
}

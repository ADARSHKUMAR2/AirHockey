using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _cancelButton;
    // [SerializeField] private int _roomSize;

    public override void OnEnable()
    {
        base.OnEnable();
        AddButtonListeners();
    }

    private void AddButtonListeners()
    {
        _startButton.onClick.AddListener(QuickStartGame);
        _cancelButton.onClick.AddListener(Cancel);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        RemoveButtonListeners();
    }

    private void RemoveButtonListeners()
    {
        _cancelButton.onClick.RemoveListener(Cancel);
        _startButton.onClick.RemoveListener(QuickStartGame);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _startButton.gameObject.SetActive(true);
    }

    public void QuickStartGame()
    {
        ShowButton(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log($"Start Game");
    }

    public override void OnJoinRandomFailed(short returnCode,string msg)
    {
        Debug.Log($"Failed to Join Room");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log($"Creating new room");
        int randomNum = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true, IsOpen = true,MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers
        };
        PhotonNetwork.CreateRoom($"Room - {randomNum}", roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string msg)
    {
        Debug.Log($"Failed to create Room");
        CreateRoom();
    }

    public void Cancel()
    {
        ShowButton(false);
        PhotonNetwork.LeaveRoom();
        Debug.Log($"Left room successfully");
    }

    private void ShowButton(bool value)
    {
        _startButton.gameObject.SetActive(!value);
        _cancelButton.gameObject.SetActive(value);
    }
}

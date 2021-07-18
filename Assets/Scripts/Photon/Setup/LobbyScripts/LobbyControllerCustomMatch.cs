using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyControllerCustomMatch : MonoBehaviourPunCallbacks,ILobbyCallbacks
{
    public static LobbyControllerCustomMatch lobby;
    public string roomName;
    public int roomSize;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;
    public List<RoomInfo> roomLists;

    private void Awake()
    {
        if (lobby == null)
            lobby = this;
        else
        {
            if (lobby != this)
            {
                Destroy(lobby.gameObject);
                lobby = this;
            }
        }
        DontDestroyOnLoad(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        AddButtonListeners();
    }

    private void AddButtonListeners()
    {
    }

    private void Start()
    {
        // PhotonNetwork.ConnectUsingSettings();
        roomLists = new List<RoomInfo>();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        RemoveButtonListeners();
    }

    private void RemoveButtonListeners()
    {
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = $"Player - {Random.Range(0,1000)}";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        // RemoveRoomListings();
        int tempIndex;
        foreach (RoomInfo room in roomList)
        {
            if (roomLists != null)
            {
                tempIndex = roomLists.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }

            if (tempIndex != -1)
            {
                roomLists.RemoveAt(tempIndex);
                Destroy(roomsPanel.GetChild(tempIndex).gameObject);
            }
            roomLists.Add(room);
            ListRoom(room);
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate(RoomInfo room)
        {
            return room.Name == name;
        };
    }

    private void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject temp = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton roomButton = temp.GetComponent<RoomButton>();
            roomButton.roomName = room.Name;
            roomButton.roomSize = room.MaxPlayers;
            roomButton.SetRoom();
        }
    }

    private void RemoveRoomListings()
    {
        int i = 0;
        while (roomsPanel.childCount!=0)
        {
            Destroy(roomsPanel.GetChild(i).gameObject);
            i++;
        }
    }

    

    public void CreateRoom()
    {
        Debug.Log($"Creating new room");
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true, IsOpen = true,MaxPlayers = (byte)roomSize
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string msg)
    {
        Debug.Log($"Failed to create Room");
        // CreateRoom();
    }

    public void OnRoomNameChanged(string name)
    {
        roomName = name;
    }
    public void OnRoomSizeChanged(string size)
    {
        roomSize =int.Parse(size);
    }

    public void JoinLobbyOnClick()
    {
        //Display list of rooms in lobby
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }
}

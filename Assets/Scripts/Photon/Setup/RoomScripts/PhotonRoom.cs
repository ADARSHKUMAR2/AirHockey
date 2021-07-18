using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks,IInRoomCallbacks
{
    [Header("Room Info")]
    public static PhotonRoom room;
    public PhotonView PV;
    public bool isGameLoaded;
    private int currentScene;
    [SerializeField] private int multiPlayScene;
    
    [Header("Player Info")]
    private Player[] photonPlayers; 
    private int totalPlayersInRoom;
    private int muNumberInRoom;
    private int playerInGame;

    [Header("Delayed Start")] 
    private bool readyToCount;
    private bool readyToStart;
    [SerializeField] private float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayer;
    private float timeToStart;
    private void Awake()
    {
        if (PhotonRoom.room == null)
            room = this;
        else
        {
            if (room != this)
            {
                Destroy(room.gameObject);
                room = this;
            }
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;

        lessThanMaxPlayers = startingTime;
        atMaxPlayer = 6;
        timeToStart = startingTime;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }
    
    private void OnSceneFinishedLoading(Scene sceneIndex, LoadSceneMode sceneMode)
    {
        
        var multiPlayerSettings = MultiplayerSettings.multiplayerSettings;
        currentScene = sceneIndex.buildIndex;
        if (currentScene == multiPlayerSettings.multiplayerScene)
        {
            isGameLoaded = true;
            if (multiPlayerSettings.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene",RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();   
            }
        }
        
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity,0);
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room successfully");
        base.OnJoinedRoom();

        if(!PhotonNetwork.IsMasterClient)
            return;
        
        photonPlayers = PhotonNetwork.PlayerList;
        totalPlayersInRoom = photonPlayers.Length;
        muNumberInRoom = totalPlayersInRoom;
        PhotonNetwork.NickName = muNumberInRoom.ToString();
        
        //Checking for delay start
        var multiPlayerSettings = MultiplayerSettings.multiplayerSettings;
        if (multiPlayerSettings.delayStart)
        {
            OnDelayStart(multiPlayerSettings);
        }
        else
        {
            StartGame();
        }

    }

    private void OnDelayStart(MultiplayerSettings multiPlayerSettings)
    {
        Debug.Log($"Display players in room out of max players possible {totalPlayersInRoom} , {multiPlayerSettings.maxPlayers}");

        if (totalPlayersInRoom > 1)
        {
            readyToCount = true;
        }

        if (totalPlayersInRoom == multiPlayerSettings.maxPlayers)
        {
            readyToStart = true;
            if (!PhotonNetwork.IsMasterClient)
                return;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"New player joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        totalPlayersInRoom++;   
        var multiPlayerSettings = MultiplayerSettings.multiplayerSettings;
        if (multiPlayerSettings.delayStart)
        {
            OnDelayStart(multiPlayerSettings);
        }
    }

    private void StartGame()
    {
        isGameLoaded = true;
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        var multiPlayerSettings = MultiplayerSettings.multiplayerSettings;
        if (multiPlayerSettings.delayStart)
            PhotonNetwork.CurrentRoom.IsOpen = false;
        
        PhotonNetwork.LoadLevel(multiPlayerSettings.multiplayerScene);
    }

    private void Update()
    {
        //Only for delay start
        var multiPlayerSettings = MultiplayerSettings.multiplayerSettings;
        if (multiPlayerSettings.delayStart)
        {
            if (totalPlayersInRoom == 1)
            {
                RestartTimer();
            }

            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayer -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayer;
                    timeToStart = atMaxPlayer;
                }
                else if(readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                
                Debug.Log($"Display time to start to the players {timeToStart}");
                if(timeToStart<=0)
                    StartGame();
            }
        }
    }

    private void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayer = 6;
        readyToCount = false;
        readyToStart = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"{otherPlayer.NickName} has left the room !");
        totalPlayersInRoom--;
    }
}

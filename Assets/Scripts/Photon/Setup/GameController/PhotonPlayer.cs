using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView PV;
    private GameObject myAvatar;
    public int myTeam;
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }
        // int spawnPicket = Random.Range(0, GameSetup.gameSetup.spawnPoints.Length);
        // if (PV.IsMine)
        // {
        //     myAvatar= PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerAvatar"),GameSetup.gameSetup.spawnPoints[spawnPicket].position,GameSetup.gameSetup.spawnPoints[spawnPicket].rotation,0);
        // }
    }

    private void Update()
    {
        if (myAvatar == null && myTeam != 0)
        {
            if (myTeam == 1)
            {
                int spawnPicket = Random.Range(0, GameSetup.gameSetup.spawnPointsTeamOne.Length);
                if (PV.IsMine)
                {
                    myAvatar= PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerAvatar"),GameSetup.gameSetup.spawnPointsTeamOne[spawnPicket].position,GameSetup.gameSetup.spawnPointsTeamOne[spawnPicket].rotation,0);
                }
            }
            else
            {
                int spawnPicket = Random.Range(0, GameSetup.gameSetup.spawnPointsTeamTwo.Length);
                if (PV.IsMine)
                {
                    myAvatar= PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerAvatar"),GameSetup.gameSetup.spawnPointsTeamTwo[spawnPicket].position,GameSetup.gameSetup.spawnPointsTeamTwo[spawnPicket].rotation,0);
                }
            }
        }
        
    }

    [PunRPC]
    private void RPC_GetTeam()
    {
        myTeam = GameSetup.gameSetup.nextTeam;
        GameSetup.gameSetup.UpdateTeam();
        PV.RPC("RPC_SentTeam",RpcTarget.OthersBuffered,myTeam);
    }

    [PunRPC]
    private void RPC_SentTeam(int whichTeam)
    {
        myTeam = whichTeam;
    }
}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;

    public int playerHealth;
    public int damage;

    private Animator anim;

    [SerializeField] private Camera myCamera;
    [SerializeField] private AudioListener myAudioClip;
    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered ,PlayerInfo.playerInfo.myCharacter);
        }
        else
        {
            Destroy(myCamera);
            Destroy(myAudioClip);
        }
    }

    [PunRPC]
    private void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.playerInfo.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
        // PhotonNetwork.InstantiateRoomObject()
    }
}

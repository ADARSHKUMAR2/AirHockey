using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCombat : MonoBehaviour
{
    private PhotonView PV;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;
    public Text healthText;
    
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();
        healthText = GameSetup.gameSetup.healthDisplay;
    }

    private void Update()
    {
        if(!PV.IsMine)
            return;
        if (Input.GetMouseButton(0))
        {
            PV.RPC("RPC_Shooting", RpcTarget.All);
        }

        Debug.Log($"health - {avatarSetup.playerHealth}");
        healthText.text = avatarSetup.playerHealth.ToString();
    }

    [PunRPC]
    private void RPC_Shooting()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out raycastHit, 1000f))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * raycastHit.distance, Color.yellow);
            Debug.Log($"Hit!!!");
            if (raycastHit.transform.CompareTag("Avatar"))
            {
                Debug.Log($"Hit another player");
                raycastHit.transform.gameObject.GetComponent<AvatarSetup>().playerHealth -= avatarSetup.damage;
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log($"Did not Hit!!!");
        }
    }
}

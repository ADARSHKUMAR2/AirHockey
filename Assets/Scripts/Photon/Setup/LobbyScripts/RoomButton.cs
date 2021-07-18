using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public Text roomNameText;
    public Text roomSizeText;
    public int roomSize;
    public string roomName;

    public void SetRoom()
    {
        roomNameText.text = roomName;
        roomSizeText.text = roomSize.ToString();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}

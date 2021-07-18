using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Observer : MonoBehaviourPunCallbacks,IPunObservable
{
    [Range(0f,500f)]
    public float brushSize;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //To Send info across n/k
            stream.SendNext(brushSize);
            Debug.Log($"Local Client {GetComponent<PhotonView>().ViewID}");
        }
        else
        {
            //To Receive info across n/k
            this.brushSize = (float) stream.ReceiveNext();
            Debug.Log($"Remote Client {GetComponent<PhotonView>().ViewID}");
        }
    }
}

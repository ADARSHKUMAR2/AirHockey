using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    public static PlayerInfo playerInfo;
    public int myCharacter;
    public GameObject[] allCharacters;

    private void OnEnable()
    {
        if (playerInfo == null)
            playerInfo = this;
        else
        {
            if (playerInfo != this)
            {
                Destroy(playerInfo.gameObject);
                playerInfo = this;
            }
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        string keyName = "MyCharacter";
        if (PlayerPrefs.HasKey(keyName))
            myCharacter = PlayerPrefs.GetInt(keyName);
        else
        {
            myCharacter = 0;
            PlayerPrefs.SetInt(keyName,myCharacter);
        }
        
            
    }
}

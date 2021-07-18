using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void ChooseCharacter(int whichChar)
    {
        if (PlayerInfo.playerInfo != null)
        {
            PlayerInfo.playerInfo.myCharacter = whichChar;
            PlayerPrefs.SetInt("MyCharacter",whichChar);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacteCustomizer : MonoBehaviour
{
    public GameObject[] hats;
    public AvatarSetUp avatar;

    public int hatParam;

    public void Update()
    {
        //ChangeHats(hatParam);
    }
    public void ChangeHats(int parameter)
    {
        PlayerData.instance.data.playerHat += parameter;
        if (PlayerData.instance.data.playerHat >= avatar.hairStyle.Length)
        {
            PlayerData.instance.data.playerHat = 0;
        }
        if (PlayerData.instance.data.playerHat < 0)
        {
            PlayerData.instance.data.playerHat = avatar.hairStyle.Length - 1;
        }
        Debug.Log(PlayerData.instance.data.playerHat);
        PlayerPrefs.SetInt("Hair",PlayerData.instance.data.playerHat);
        avatar.HairUpdater();
    }
}

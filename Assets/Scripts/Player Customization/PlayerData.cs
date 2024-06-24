using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public Data data;
  
    private void OnEnable()
    {
        data = new Data();
        PlayerData.instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string AvatarToString()
    {
        string returnString = JsonUtility.ToJson(PlayerData.instance.data);
        return returnString;
    }

    public void SetCostume()
    {
        PlayerData.instance.data.playerHat = 3;

    }
}

public class Data
{
    public static PlayerData instance;
    public int playerHat;
}


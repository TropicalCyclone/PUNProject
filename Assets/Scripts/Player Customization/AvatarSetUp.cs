using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AvatarSetUp : MonoBehaviour
{
    PhotonView myPV;
    public GameObject[] hairStyle;
    public int hairIndex;
    public int CurrentHairIndex
    {
        get { return hairIndex; }
        set
        {
            if (value >= 0 && value < hairStyle.Length)
            {
                hairIndex = value;
                UpdateHairStyle();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        if (myPV.IsMine)
        {
            hairIndex = hairStyle.Length;
            CurrentHairIndex = PlayerData.instance.data.playerHat;
            UpdateHairStyle();
        }
    }

    private void Update()
    {
        // This can be removed if not used
        // CurrentHairIndex = hairIndex;
    }

    public void UpdateHairStyle()
    {
        for (int i = 0; i < hairStyle.Length; i++)
        {
            hairStyle[i].SetActive(i == CurrentHairIndex);
        }
    }

    public void HairUpdater()
    {
        int newHairIndex = PlayerData.instance.data.playerHat;
        if (newHairIndex != CurrentHairIndex)
        {
            hairStyle[CurrentHairIndex].SetActive(false);
            CurrentHairIndex = newHairIndex;
            hairStyle[CurrentHairIndex].SetActive(true);
        }
    }

    public void SetAvatar(Data avatarData)
    {
        Debug.Log(avatarData.playerHat);
        if (avatarData.playerHat >= 0 && avatarData.playerHat < hairStyle.Length)
        {
            if (CurrentHairIndex >= 0 && CurrentHairIndex < hairStyle.Length)
            {
                hairStyle[CurrentHairIndex].SetActive(false);
            }
            
         
            hairStyle[avatarData.playerHat].SetActive(true);
            CurrentHairIndex = avatarData.playerHat;
        }
    }
}

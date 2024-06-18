using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput, joinInput, Nickname;
    public TMP_Text ErrorName;
    public byte maxPlayers;
    // Start is called before the first frame update
    public void CreateBTN()
    {
        if (PhotonNetwork.NickName != "")
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayers;
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
        else
        {
            ErrorName.text = "Name is Required when Creating and Joining games.";
        }
    }

    public void JoinBTN()
    {
        if (PhotonNetwork.NickName != "")
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            ErrorName.text = "Name is Required when Creating and Joining games.";
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("failed");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Game");
        PhotonNetwork.LoadLevel("Game 2");
    }

    public void SetNickname()
    {
        if (Nickname.text != "")
        {
            PhotonNetwork.NickName = Nickname.text;
            ErrorName.text = "";
        }
        else
        {
            ErrorName.text = "Name space should not be empty";
        }
    }
}

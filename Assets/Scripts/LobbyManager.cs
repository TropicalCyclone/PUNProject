using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput, joinInput;
    public byte maxPlayers;
    // Start is called before the first frame update
    public void CreateBTN()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(createInput.text,roomOptions);
    }

    public void JoinBTN()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("failed");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Game");
        PhotonNetwork.LoadLevel("Game");
    }
}

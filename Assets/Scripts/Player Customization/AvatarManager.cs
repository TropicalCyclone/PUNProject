using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AvatarManager : MonoBehaviour
{
    [SerializeField]private AvatarSetUp avatar;
    private Data myData;

    private PhotonView view;

    void Start()
    {
        avatar = GetComponent<AvatarSetUp>();
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            avatar.SetAvatar(PlayerData.instance.data);
            Debug.Log(PlayerData.instance.data.playerHat);
            SyncAvatar(PlayerData.instance);
        }
    }

    public void SyncAvatar(PlayerData data)
    {
        string syncString = data.AvatarToString();
        view.RPC("RPC_SyncAvatar", RpcTarget.OthersBuffered, syncString);
    }

    [PunRPC]
    void RPC_SyncAvatar(string data)
    {
        Debug.Log("RPC_SyncAvatar called with data: " + data);
        if (data == null)
        {
            Debug.LogError("Received null data in RPC_SyncAvatar.");
            return;
        }

        myData = JsonUtility.FromJson<Data>(data);
        if (myData == null)
        {
            Debug.LogError("Deserialized myData is null.");
            return;
        }

        if (avatar == null)
        {
            Debug.LogError("Avatar component is not assigned.");
            return;
        }

        Debug.Log("Setting avatar with deserialized data.");
        avatar.SetAvatar(myData);
    }
}
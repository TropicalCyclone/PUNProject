using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class ChatManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject messagePrefab;
    [SerializeField] GameObject Content;


    public void SendMessage()
    {
        string messaage = inputField.text;
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, (PhotonNetwork.NickName + " : " + messaage));
        inputField.text = "";
    }
    [PunRPC]
    public void GetMessage(string RecievedMessage)
    {
        GameObject M = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, Content.transform);
        M.GetComponent<MessageChat>().message.text = RecievedMessage;
    }
}

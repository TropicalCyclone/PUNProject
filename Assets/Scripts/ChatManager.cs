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

    private bool isTyping = false;

    public bool GetIsTyping()
    {
        return isTyping;
    }
     
    void Update()
    {
        // Check if the user is typing
        if (isTyping)
        {
            // Check if the user presses Enter while typing to send the message
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SendMessage();
            }
            // Check if the user presses Escape to stop typing
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopTyping();
            }
        }
        else
        {
            // Check if the user presses "/" or "T" to start typing
            if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.T))
            {
                StartTyping();
            }
        }
    }

    void StartTyping()
    {
        isTyping = true;
        inputField.Select();
        inputField.text = ""; // Clear any existing text
        ToggleCursorLock(false); // Unlock the cursor
    }

    void StopTyping()
    {
        isTyping = false;
        inputField.text = ""; // Clear any text in the input field
        ToggleCursorLock(true); // Lock the cursor
    }

    public void SendMessage()
    {
        if (string.IsNullOrEmpty(inputField.text)) return;

        string message = inputField.text;
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + message);
        inputField.text = "";
        isTyping = false;
        ToggleCursorLock(true); // Lock the cursor again
    }

    void ToggleCursorLock(bool shouldLock)
    {
        if (shouldLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    [PunRPC]
    void GetMessage(string receivedMessage)
    {
        GameObject newMessage = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, Content.transform);
        newMessage.GetComponent<MessageChat>().message.text = receivedMessage;
    }
}

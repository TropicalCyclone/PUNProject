using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using JetBrains.Annotations;

public class SpawnPlayerNew : MonoBehaviour
{
    public GameObject Player;
    public Transform SpawnPoint;
    public GameObject Camera;
    public CinemachineFreeLook freeLook;
    public UIManager uiManager;
    public ChatManager chatManager;

    private void Start()
    {
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        GameObject User = PhotonNetwork.Instantiate(Player.name, SpawnPoint.position, SpawnPoint.rotation);
        if (User.GetPhotonView().IsMine)
        {
            PlayerMovement manager = User.GetComponent<PlayerMovement>();
            PlayerGrab playerGrab = User.GetComponent<PlayerGrab>();
            manager.SetCamera(Camera);
            freeLook.Follow = manager.transform;
            freeLook.LookAt = manager.transform;
            playerGrab._uiManager = uiManager;
            manager._UImanager = uiManager;
            manager._Chatmanager = chatManager;

            // Add PlayerCheckpointTracker component
            User.AddComponent<PlayerCheckpointTracker>();
        }
    }
}
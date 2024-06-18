using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class SpawnPlayerNew : MonoBehaviour
{
    public GameObject Player;
    public Transform SpawnPoint;
    public GameObject Camera;
    public CinemachineFreeLook freeLook;
    public UIManager uiManager;

    // Update is called once per frame
    void Update()
    {
    }

    private void Start()
    {
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        GameObject User = PhotonNetwork.Instantiate(Player.name, SpawnPoint.position,SpawnPoint.rotation);
        PlayerMovement manager = User.GetComponent<PlayerMovement>();
        PlayerGrab playerGrab = User.GetComponent<PlayerGrab>();
        if (User.GetPhotonView().IsMine)
        {
            manager.SetCamera(Camera);
            freeLook.Follow = manager.transform;
            freeLook.LookAt = manager.transform;
            playerGrab._uiManager = uiManager;
            
            return;
        }
       

    }
}

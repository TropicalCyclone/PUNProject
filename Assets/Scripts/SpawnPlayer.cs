using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject Player;
    public Transform SpawnPoint;
    public HUD hud;
    public Inventory inventory;
    public GameManager gameManager;
    [SerializeField] PlayerFollow playerfollow;

    private Vector3 lastCheckpoint;

    void Update()
    {
        playerfollow = FindAnyObjectByType<PlayerFollow>();
    }

    private void Start()
    {
        lastCheckpoint = SpawnPoint.position;
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        GameObject User = PhotonNetwork.Instantiate(Player.name, lastCheckpoint, SpawnPoint.rotation);
        if (User.GetPhotonView().IsMine)
        {
            PlayerController controller = User.GetComponent<PlayerController>();
            gameManager.Player = controller;
            if (controller)
            {
                controller.Hud = hud;
                controller.Inventory = inventory;
            }
            if (playerfollow)
                playerfollow.SetCameraTarget(User.transform);
            else
            {
                Debug.Log("did not link camera to player");
            }
        }
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpoint = checkpointPosition;
    }

    public void RespawnAtCheckpoint()
    {
        if (gameManager.Player != null)
        {
            gameManager.Player.transform.position = lastCheckpoint;
        }
        else
        {
            SpawnPlayers();
        }
    }
}
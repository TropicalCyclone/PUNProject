using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject Player;
    public Transform SpawnPoint;

    [SerializeField]PlayerFollow playerfollow;

    // Update is called once per frame
    void Update()
    {
        playerfollow = FindAnyObjectByType<PlayerFollow>();
    }

    private void Start()
    {
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
       
        GameObject User = PhotonNetwork.Instantiate(Player.name, SpawnPoint.position,SpawnPoint.rotation);
        if (User.GetPhotonView().IsMine)
        {
            if (playerfollow)
                playerfollow.SetCameraTarget(User.transform);
            else
            {
                Debug.Log("did not link camera to player");
            }
        }
    }
}

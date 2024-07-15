using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;


public class CheckpointManager : MonoBehaviourPunCallbacks
{
    public static CheckpointManager Instance;
    private Dictionary<int, Vector3> playerCheckpoints = new Dictionary<int, Vector3>();
    [SerializeField] private float resetYThreshold = -10f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPlayer(int playerID, Vector3 initialPosition)
    {
        if (!playerCheckpoints.ContainsKey(playerID))
        {
            playerCheckpoints[playerID] = initialPosition;
        }
    }

    public void SetCheckpoint(int playerID, Vector3 position)
    {
        if (playerCheckpoints.ContainsKey(playerID))
        {
            playerCheckpoints[playerID] = position;
        }
    }

    public void CheckPlayerPosition(int playerID, Transform playerTransform)
    {
        if (playerTransform.position.y < resetYThreshold)
        {
            ResetPlayerToCheckpoint(playerID, playerTransform);
        }
    }

    private void ResetPlayerToCheckpoint(int playerID, Transform playerTransform)
    {
        if (playerCheckpoints.TryGetValue(playerID, out Vector3 checkpoint))
        {
            playerTransform.position = checkpoint;
            Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
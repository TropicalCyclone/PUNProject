using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class FallingObjectSpawner : MonoBehaviourPunCallbacks
{
    public GameObject fallingObjectPrefab;
    public List<Transform> spawnPoints;
    public float spawnInterval = 2f;
    public float initialDelay = 1f;

    private void Start()
    {
        Debug.Log("FallingObjectSpawner Start method called");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("This client is the Master Client, starting spawn routine");
            InvokeRepeating("SpawnFallingObject", initialDelay, spawnInterval);
        }
        else
        {
            Debug.Log("This client is not the Master Client, not spawning objects");
        }
    }

    private void SpawnFallingObject()
    {
        if (spawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];

            Debug.Log($"Attempting to spawn object at {spawnPoint.position}");
            GameObject spawnedObject = PhotonNetwork.Instantiate(fallingObjectPrefab.name, spawnPoint.position, Quaternion.identity);
            if (spawnedObject != null)
            {
                Debug.Log("Object successfully spawned");
            }
            else
            {
                Debug.LogError("Failed to spawn object");
            }
        }
        else
        {
            Debug.LogError("No spawn points assigned to FallingObjectSpawner");
        }
    }
}
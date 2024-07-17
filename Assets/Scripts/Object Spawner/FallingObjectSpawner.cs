using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace YourGameNamespace
{
    public class FallingObjectSpawner : MonoBehaviourPunCallbacks
    {
        public GameObject fallingObjectPrefab;
        public List<Transform> spawnPoints;
        public float spawnInterval = 2f;
        public float initialDelay = 1f;

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating("SpawnFallingObject", initialDelay, spawnInterval);
            }
        }

        private void SpawnFallingObject()
        {
            if (spawnPoints.Count > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Count);
                Transform spawnPoint = spawnPoints[randomIndex];

                PhotonNetwork.Instantiate(fallingObjectPrefab.name, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No spawn points assigned to FallingObjectSpawner");
            }
        }
    }
}
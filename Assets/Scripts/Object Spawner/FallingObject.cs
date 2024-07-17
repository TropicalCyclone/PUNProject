using UnityEngine;
using Photon.Pun;

public class FallingObject : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Falling object hit the ground");

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Falling object hit a player");
            // Lagay ng player logic damage
        }
    }
}
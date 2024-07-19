using UnityEngine;
using Photon.Pun;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                CheckpointManager.Instance.SetCheckpoint(photonView.ViewID, transform.position);
            }
            Destroy(this.gameObject);
        }
    }
}
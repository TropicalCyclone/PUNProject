using UnityEngine;
using Photon.Pun;

public class PlayerCheckpointTracker : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (photonView.IsMine)
        {
            CheckpointManager.Instance.RegisterPlayer(photonView.ViewID, transform.position);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            CheckpointManager.Instance.CheckPlayerPosition(photonView.ViewID, transform);
        }
    }
}
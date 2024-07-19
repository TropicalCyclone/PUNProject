using UnityEngine;
using Photon.Pun;
using System.Threading;

public class PlayerCheckpointTracker : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (photonView.IsMine)
        {
            CheckpointManager.Instance.RegisterPlayer(photonView.ViewID, this.transform.position);
            Debug.Log("Trying to Register Player" + photonView.ViewID);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            CheckpointManager.Instance.CheckPlayerPosition(photonView.ViewID, this.transform);
        }
    }
}
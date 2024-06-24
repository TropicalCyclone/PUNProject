using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerGrab grab;
    [SerializeField] private PlayerMovement player;
    private PhotonView view;
    //[SerializeField] private AudioManager audioManager;

    private bool only_once;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (player.GetWalkingStatus())
            {
                animator.SetBool("run", true);
            }
            else
            {
                animator.SetBool("run", false);
            }

            if (player.GetCrouchStatus())
            {
                // animator.SetBool("IsSneaking", true);
            }
            else
            {
                // animator.SetBool("IsSneaking", false);
            }

            if (player.IsInAir())
            {
                animator.SetBool("is_in_air", true);
            }
            else
            {
                animator.SetBool("is_in_air", false);
            }

            if (grab.GetPickupStatus())
            {
                if (!only_once)
                {
                    animator.SetTrigger("tr_pickup");
                    only_once = true;
                }
            }
            else if (grab.GetDropStatus())
            {
                if (!only_once)
                {
                    animator.SetTrigger("tr_drop");
                    only_once = true;
                }
            }

            else
            {
                only_once = false;
            }
        }
    }

    public void PickupAnimationSet()
    {
        if(view.IsMine) 
        grab.PickUpItem();
    }

    public void DropAnimationSet()
    {
        if(view.IsMine)
        grab.DropItem();
    }
    public void grabItem()
    {
        //audioManager.grabSoundPlay();
    }
}

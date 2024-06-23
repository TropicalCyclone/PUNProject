using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.Timeline;

public class PlayerGrab : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("The place you want to make the player grab the object")]
    private Transform _hand;

    [SerializeField] private BaseItem _handObject;
    [SerializeField] private ItemManager _itemManager;
    [SerializeField] private float _pickupMaximum = 2f;
    [SerializeField] public UIManager _uiManager;
    [SerializeField] private PhotonView _view;

    private float _pickupDistance;
    private float _distance;
    private BaseItem lastBaseItem;
    private BaseItem currentBaseItem;
    private bool _grabStatus = true;
    private bool _pickupStatus;
    private bool _dropStatus;
    private BaseItem _serverObject;
    

    public bool isObjectGrabbed()
    {
        return _handObject != null;
    }

    private void Awake()
    {
        if (!_view)
        {
            _view = GetComponent<PhotonView>();
        }
        if (_view.IsMine)
        {
            if (!_itemManager)
            {
                _itemManager = FindObjectOfType<ItemManager>(); // Corrected typo in method name
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine && _grabStatus)
        {
            UIUpdate();

            if (Input.GetKeyDown(KeyCode.E))
            {
                _pickupDistance = 2f;

                if (_hand.childCount <= 0)
                {
                    _handObject = null;
                }

                if (!_handObject)
                {
                    _pickupStatus = true;
                }
                else
                {
                    _dropStatus = true;
                }
                _pickupDistance = _distance;
            }
            else
            {
                _pickupStatus = false;
                _dropStatus = false;
            }

            
            // Continuously teleport the hand object to the player's hand
            //if (_handObject != null)
            //{
            //    _handObject.transform.position = _hand.position;
            //    _handObject.transform.rotation = _hand.rotation;
            //}
        }
    }

    public bool GetPickupStatus()
    {
        return _pickupStatus;
    }

    public bool GetDropStatus()
    {
        return _dropStatus;
    }

    public void PickUpItem()
    {
        if (_view.IsMine)
        {
            _handObject = currentBaseItem;
            if (_handObject)
            {
                _itemManager.RemoveItem(_handObject);

                if (_handObject.GetItemCollider())
                {
                    _handObject.ToggleRigidBody(true);
                    _handObject.GetItemCollider().enabled = false;
                }
                _handObject.transform.SetParent(_hand);
                _handObject.transform.localPosition = Vector3.zero;
                _handObject.transform.localRotation = Quaternion.identity;

                photonView.RPC("SyncHandObject", RpcTarget.OthersBuffered, _handObject.gameObject.GetPhotonView().ViewID,false);
            }
        }
    }

    [PunRPC]
    private void SyncHandObject(int viewID,bool Drop)
    {
        PhotonView.Find(viewID).TransferOwnership(PhotonNetwork.LocalPlayer);
        GameObject obj = PhotonView.Find(viewID).gameObject;
        _serverObject = obj.GetComponent<BaseItem>();
        if (Drop)
        {
                _serverObject.transform.SetParent(null);
                _serverObject.ToggleRigidBody(false);
                _serverObject.GetItemCollider().enabled = true;
        }
        else
        {
            
            _serverObject.ToggleRigidBody(true);
            _serverObject.GetItemCollider().enabled = false;
            obj.GetComponent<Collider>().enabled = false;
            obj.GetComponent<Rigidbody>().isKinematic = true;
            _serverObject.transform.SetParent(_hand);
            _serverObject.transform.localPosition = Vector3.zero;
            _serverObject.transform.localRotation = Quaternion.identity;
        }
    }

    public void DropItem()
    {
        if (_view.IsMine && _handObject)
        {
            if (_handObject.GetItemCollider())
            {
                _handObject.GetItemCollider().enabled = true;
                _handObject.ToggleRigidBody(false);
            }
            _handObject.transform.SetParent(null);
            _itemManager.AddItem(_handObject);
            int viewID = _handObject.gameObject.GetPhotonView().ViewID;
            _handObject = null;

            photonView.RPC("SyncHandObject", RpcTarget.OthersBuffered,viewID,true);
        }
    }

    public void UIUpdate()
    {
        if (_uiManager != null)
        {
            currentBaseItem = PickClosestObject();
            if (currentBaseItem != lastBaseItem)
            {
                if (!_handObject)
                {
                    _uiManager.SetGrabVisual(true);
                    _uiManager.SetText("Pick Up");
                }
                if (!currentBaseItem)
                {
                    _uiManager.SetGrabVisual(false);
                }
                lastBaseItem = currentBaseItem;
            }
        }
    }

    public void DisableControl()
    {
        _grabStatus = false;
    }

    public void EnableControl()
    {
        _grabStatus = true;
    }

    private BaseItem PickClosestObject()
    {
        BaseItem itemDetect = null;
        _pickupDistance = _pickupMaximum;
        HashSet<BaseItem> Items = _itemManager.GetItems();

        foreach (BaseItem item in Items)
        {
            if (item)
            {
                _distance = Vector3.Distance(transform.position, item.transform.position);
                if (_distance < _pickupDistance)
                {
                    _pickupDistance = _distance;
                    itemDetect = item;
                }
            }
        }
        return itemDetect;
    }
}

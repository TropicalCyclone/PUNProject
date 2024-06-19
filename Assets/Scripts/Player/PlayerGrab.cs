using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField, Tooltip("The Place you want to make the player grab the object")] private Transform _hand;
    [SerializeField] private BaseItem _handObject;
    [SerializeField] private ItemManager _itemManager;
    [SerializeField] private float _pickupMaximum = 2f;
    [SerializeField] public UIManager _uiManager;
    private float _pickupDistance;
    private float _distance;
    private BaseItem lastBaseItem;
    private BaseItem currentBaseItem;
    private PhotonView _view;
    [SerializeField] private bool _grabStatus = true;
    [SerializeField] private bool _pickupStatus;
    [SerializeField] private bool _dropStatus;

    public bool isObjectGrabbed()
    {
        return _handObject;
    }

    void Awake()
    {
        _view = GetComponent<PhotonView>();
        if (_view.IsMine)
        {
            if (!_itemManager)
            {
                _itemManager = FindAnyObjectByType<ItemManager>();
            }
        }
    }

    public BaseItem GetHandBaseItem()
    {
        return _handObject;
    }

    void Update()
    {
        if (_view.IsMine)
        {
            UIUpdate();
            if (_grabStatus)
            {
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
                if (_handObject != null)
                {
                    _handObject.transform.position = _hand.position;
                    _handObject.transform.rotation = _hand.rotation;
                }
            }
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
                _handObject.transform.position = _hand.position;
                _handObject.transform.rotation = _hand.rotation;
            }
        }
    }

    public void DropItem()
    {
        if (_view.IsMine)
        {
            if (_handObject)
            {
                if (_handObject.GetItemCollider())
                {
                    _handObject.GetItemCollider().enabled = true;
                    _handObject.ToggleRigidBody(false);
                }
                _handObject.transform.position = _hand.position;
                _handObject.transform.rotation = _hand.rotation;
                _itemManager.AddItem(_handObject);
                _handObject = null;
            }
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

    BaseItem PickClosestObject()
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

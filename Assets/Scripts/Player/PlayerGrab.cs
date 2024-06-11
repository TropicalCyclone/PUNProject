using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField,Tooltip("The Place you want to make the player grab the object")] private Transform _hand;
    [SerializeField] private BaseItem _handObject;
    [SerializeField] private ItemManager _itemManager;
    [SerializeField] private float _pickupMaximum = 2f;
    [SerializeField] private UIManager _uiManager;
    private float _pickupDistance;
    private float _distance;
    private BaseItem lastBaseItem;
    private BaseItem currentBaseItem;

    public bool isObjectGrabbed()
    {
        return _handObject;
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (!_itemManager)
        {
            _itemManager = FindAnyObjectByType<ItemManager>();
        }
    }

    public BaseItem GetHandBaseItem()
    {
        return _handObject;
    }
        // Update is called once per frame
    void Update()
    {
        UIUpdate();

        if (Input.GetKeyDown(KeyCode.E))
        {
            _pickupDistance = 2f;
            if(_hand.childCount <= 0)
            {
                _handObject = null;
            }
            if (!_handObject)
            {
                PickUpItem();
            }
            else
            {
                DropItem();
            }
            _pickupDistance = _distance;
                
            
        }   
    }

    public void PickUpItem()
    {
        _handObject = currentBaseItem;
        if (_handObject)
        {
            _itemManager.RemoveItem(_handObject);
            _handObject.ToggleRigidBody(true);
            _handObject.transform.parent = _hand;
            _handObject.transform.localPosition = Vector3.zero;
            _handObject.transform.localRotation = Quaternion.identity;
            _handObject.GetItemCollider().enabled = false;
        }
    }

    public void DropItem()
    {
        _handObject.GetItemCollider().enabled = true;
        _handObject.ToggleRigidBody(false);
        _handObject.transform.parent = null;
        _itemManager.AddItem(_handObject);
        _handObject = null;
    }
    public void UIUpdate()
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
    

    BaseItem PickClosestObject()
    {
        BaseItem itemDetect = null;
        _pickupDistance =  _pickupMaximum;
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



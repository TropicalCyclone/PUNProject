using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private HashSet<BaseItem> _items = new(); 
   
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        { 
            _items.Add(child.gameObject.GetComponentInChildren<BaseItem>());
        }
    }

    public HashSet<BaseItem> GetItems()
    {
        return _items;
    }

    public void RemoveItem(BaseItem item)
    {
        if (_items.Contains(item))
        {
            _items.Remove(item);
        }
    }

    public void AddItem(BaseItem item)
    {
        _items.Add(item);
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    private void OnTransformChildrenChanged()
    {
        
    }
}

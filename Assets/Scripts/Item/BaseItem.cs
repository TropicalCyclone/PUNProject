using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string itemName;
    [SerializeField]
    [TextArea]
    private string description;
    [SerializeField]
    private Sprite itemImage;
    private GameObject itemGameObject;
    [SerializeField]
    private Rigidbody itemRigidbody;
    [SerializeField]
    private Collider itemCollider;

    private void OnEnable()
    {
        itemGameObject = gameObject;
    }

  public Rigidbody GetItemRigidbody()
    {
        return itemRigidbody;
    }

    public void ToggleRigidBody(bool value)
    {
        if(itemRigidbody != null)
            itemRigidbody.isKinematic = value;
    }
  public Collider GetItemCollider()
    {
        return itemCollider;
    }

}

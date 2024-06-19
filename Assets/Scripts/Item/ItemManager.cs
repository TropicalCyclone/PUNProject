using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemManager : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class ItemSpawnPoint
    {
        public BaseItem baseItemPrefab; // Prefab of the BaseItem
        public Transform spawnPoint;
    }

    [SerializeField] private List<ItemSpawnPoint> itemSpawnPoints = new List<ItemSpawnPoint>();
    private HashSet<BaseItem> _items = new HashSet<BaseItem>();
    private PhotonView view; // Reference to the PhotonView component

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>(); // Get the PhotonView component on this GameObject

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnItems();
        }
    }

    public HashSet<BaseItem> GetItems()
    {
        return _items;
    }

    public void RemoveItem(BaseItem item)
    {
        if (item != null && _items.Contains(item))
        {
            _items.Remove(item);
            var itemPhotonView = item.GetComponent<PhotonView>();
            if (itemPhotonView != null)
            {
                view.RPC("RemoveItemRPC", RpcTarget.OthersBuffered, itemPhotonView.ViewID);
            }
        }
    }

    [PunRPC]
    public void RemoveItemRPC(int itemViewID)
    {
        var item = PhotonView.Find(itemViewID)?.GetComponent<BaseItem>();
        if (item != null && _items.Contains(item))
        {
            _items.Remove(item);
        }
    }

    public void AddItem(BaseItem item)
    {
        if (item != null)
        {
            _items.Add(item);
            var itemPhotonView = item.GetComponent<PhotonView>();
            if (itemPhotonView != null)
            {
                view.RPC("AddItemRPC", RpcTarget.OthersBuffered, itemPhotonView.ViewID);
            }
        }
    }

    [PunRPC]
    public void AddItemRPC(int itemViewID)
    {
        var item = PhotonView.Find(itemViewID)?.GetComponent<BaseItem>();
        if (item != null)
        {
            _items.Add(item);
        }
    }

    public void SpawnItems()
    {
        foreach (var itemSpawn in itemSpawnPoints)
        {
            if (itemSpawn.baseItemPrefab == null || itemSpawn.spawnPoint == null)
                continue;

            GameObject itemInstance = PhotonNetwork.Instantiate(itemSpawn.baseItemPrefab.name, itemSpawn.spawnPoint.position, itemSpawn.spawnPoint.rotation);
            var baseItem = itemInstance.GetComponent<BaseItem>();
            if (baseItem != null)
            {
                AddItem(baseItem);
            }
            else
            {
                Debug.LogError("The instantiated object does not have a BaseItem component.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTransformChildrenChanged()
    {
    }
}

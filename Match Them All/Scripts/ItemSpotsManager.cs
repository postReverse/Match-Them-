using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.UIElements;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform ItemSpotParent;
    private ItemSpot[] spots;


    [Header("Settings")]
    [SerializeField] private Vector3 ItemLocalPositionOnSpot;
    [SerializeField] private Vector3 ItemLocalScaleOnSpot;
    private bool isBusy;

    [Header("Data")]
    private Dictionary<EItemName,ItemMergeData> itemMergeDataDictionary = new Dictionary<EItemName,ItemMergeData>();



    private void Awake()
    {
        ImputManager.ItemClicked += OnItemClicked;

        StoreSpots();

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnItemClicked(Item item)
    {

        if (isBusy)
        {
            Debug.LogWarning("ItemSpotsManager is busy...");
            return;
        }

        if ( !IsFreeSpotAvailable())
        {
            Debug.LogWarning("No free spots available! ");
            return;
        }

        isBusy = true; 

        HandleItemClicked(item);
        
    }

    private void HandleItemClicked(Item item)
    {

        if (itemMergeDataDictionary.ContainsKey(item.ItemName))
            HandleItemMergeDataFound(item); 
        else
            MoveItemToFirstFreeSpot(item);
    }

    private void HandleItemMergeDataFound(Item item)
    {
        ItemSpot idealSpot = GetIdealSpotFor(item);

        itemMergeDataDictionary[item.ItemName].Add(item);

        TryMoveItemToIdealSpot(item, idealSpot);
    }

    private ItemSpot GetIdealSpotFor(Item item)
    {
        List<Item> items = itemMergeDataDictionary[item.ItemName].items;
        List<ItemSpot> itemSpots = new List<ItemSpot>();

        for (int i = 0; i < items.Count; i++)
            itemSpots.Add(items[i].Spot);

        // we have a list of occupied spots by the items similar to item

        //if we have only one spot, we should simply grab the spot next to it
        if (itemSpots.Count >= 2)
        {
            itemSpots.Sort((a,b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));
        }

        int idealSpotIndex = itemSpots[0].transform.GetSiblingIndex() + 1;

        return spots[idealSpotIndex];

    }

    private void TryMoveItemToIdealSpot(Item item, ItemSpot IdealSpot)
    {
        
        if ( !IdealSpot.IsEmpty())
        {
            HandleIdealSpotFull(item, IdealSpot);
            return;
        }

        MoveItemToSpot(item, IdealSpot, () => HandleItemReachedSpot(item));

    }

    private void MoveItemToSpot(Item item, ItemSpot targetSpot, Action completeCallBack)
    {

        targetSpot.Populate(item);

        //scale the item down, set it's local position 0 0 0 
        item.transform.localPosition = ItemLocalPositionOnSpot;
        item.transform.localScale = ItemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        // disable it's shadow
        item.DisableShadows();

        // disable it's collider / physics
        item.DisablePhysics();

        completeCallBack?.Invoke();

        //HandleItemReachedSpot(item, checkForMerge);

    }

    private void HandleItemReachedSpot(Item item, bool checkForMerge = true )
    {
        if (!checkForMerge) return;

        if (itemMergeDataDictionary[item.ItemName].CanMergeItems())
            MergeItems(itemMergeDataDictionary[item.ItemName]);
        else
            CheckForGameOver();
    }

    private void MergeItems(ItemMergeData itemMergeData)
    {
        List<Item> items = itemMergeData.items;

        //remove the item merge data from the dictionary
        itemMergeDataDictionary.Remove(itemMergeData.itemName);

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Spot.Clear();
            Destroy(items[i].gameObject);
        }

        MoveAllItemsToTheLeft();

        // TODO: Remove this line after moving the items to the left!
        //isBusy = false; 

    }

    private void MoveAllItemsToTheLeft()
    {
        for ( int i = 3; i<spots.Length; i++)
        {
            ItemSpot spot = spots[i];
            if (spot.IsEmpty())
                continue;


            Item item = spot.Item;

            ItemSpot targetSpot = spots[i - 3];

            if(!targetSpot.IsEmpty())
            {
                Debug.LogWarning(targetSpot.name + " is full");
                isBusy = false;
                return;
            }

            spot.Clear();
            MoveItemToSpot(item, targetSpot, () => HandleItemReachedSpot(item,false));
        }

        HandleAllItemsMovedToTheLeft();

    }

    private void HandleAllItemsMovedToTheLeft()
    {
        isBusy = false; 
    }

    private void HandleIdealSpotFull(Item item, ItemSpot idealSpot)
    {
        MoveAllItemsToTheRightFrom(idealSpot, item);
    }

    private void MoveAllItemsToTheRightFrom(ItemSpot idealSpot, Item itemToPlace)
    {
        int spotIndex = idealSpot.transform.GetSiblingIndex();

        for ( int i = spots.Length-2; i>=spotIndex; i--)
        {
            ItemSpot spot = spots[i];

            if (spot.IsEmpty())
                continue;

            Item item = spot.Item;
            spot.Clear();
            ItemSpot targetSpot = spots[i + 1];

            if (!targetSpot.IsEmpty())
            {
                Debug.LogWarning("ERROR! This should not happen");
                isBusy = false;
                return;
            }

            MoveItemToSpot(item, targetSpot, ()=> HandleItemReachedSpot(item, false));
        }
        MoveItemToSpot(itemToPlace, idealSpot, ()=> HandleItemReachedSpot(itemToPlace));
    }

    private void MoveItemToFirstFreeSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();

        if (targetSpot == null )
        {
            Debug.LogError("Target spot is null => this should not happen!");
            return;
        }

        CreateItemMergeData(item);

        MoveItemToSpot(item, targetSpot, ()=> HandleFirstItemReachedSpot(item));

    }

    private void HandleFirstItemReachedSpot(Item item)
    {
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (GetFreeSpot() == null)
            Debug.LogWarning("Game Over");
        else
            isBusy = false;
    }

    private void CreateItemMergeData(Item item)
    {
        itemMergeDataDictionary.Add(item.ItemName, new ItemMergeData(item));
    }

    private ItemSpot GetFreeSpot() 
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].IsEmpty())
                return spots[i];
        }
        return null;
    }

    private void OnDestroy()
    {
        ImputManager.ItemClicked -= OnItemClicked;
    }

    private bool IsFreeSpotAvailable()
    {
        for ( int i = 0; i < spots.Length; i++ )
        {
            if (spots[i].IsEmpty()) {
                return true;
            }
        }
        return false;
    }

    private void StoreSpots()
    {
        spots = new ItemSpot[ItemSpotParent.childCount];
        for (int i = 0; i < ItemSpotParent.childCount; i++)
            spots[i] = ItemSpotParent.GetChild(i).GetComponent<ItemSpot>();
        
    }

}


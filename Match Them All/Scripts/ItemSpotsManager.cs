using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform ItemSpotParent;
    private ItemSpot[] spots;


    [Header("Settings")]
    [SerializeField] private Vector3 ItemLocalPositionOnSpot;
    [SerializeField] private Vector3 ItemLocalScaleOnSpot;

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

        if ( !IsFreeSpotAvailable())
        {
            Debug.LogWarning("No free spots available! ");
            return;
        }


        HandleItemClicked(item);
        
    }

    private void HandleItemClicked(Item item)
    {
        MoveItemToFirstFreeSpot(item);
    }

    private void MoveItemToFirstFreeSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();

        if (targetSpot == null )
        {
            Debug.LogError("Target spot is null => this should not happen!");
            return;
        }

        targetSpot.Populate(item);

        //scale the item down, set it's local position 0 0 0 
        item.transform.localPosition = ItemLocalPositionOnSpot;
        item.transform.localScale = ItemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        // disable it's shadow
        item.DisableShadows();

        // disable it's collider / physics
        item.DisablePhysics();

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


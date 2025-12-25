using UnityEngine;
using System;

public class ItemSpotsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform ItemSpot;

    [Header("Settings")]
    [SerializeField] private Vector3 ItemLocalPositionOnSpot;
    [SerializeField] private Vector3 ItemLocalScaleOnSpot;

    private void Awake()
    {
        ImputManager.ItemClicked += OnItemClicked;
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
        // turn the item as a child of the item spot
        item.transform.SetParent(ItemSpot);

        //scale the item down, set it's local position 0 0 0 
        item.transform.localPosition = ItemLocalPositionOnSpot;
        item.transform.localScale = ItemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        // disable it's shadow
        item.DisableShadows();

        // disable it's collider / physics
        item.DisablePhysics();
    }

    private void OnDestroy()
    {
        ImputManager.ItemClicked -= OnItemClicked;
    }

}

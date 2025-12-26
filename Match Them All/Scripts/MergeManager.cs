using UnityEngine;
using System.Collections.Generic;
using System;

public class MergeManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float goUpDistance;
    [SerializeField] private float goUpDuration;
    [SerializeField] private LeanTweenType goUpEasing;

    private void Awake()
    {
        ItemSpotsManager.MergeStarted += OnMergeStarted;
    }

    private void OnDestroy()
    {
        ItemSpotsManager.MergeStarted -= OnMergeStarted;
    }

    private void OnMergeStarted(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {

            Vector3 targetPosition = items[i].transform.position + items[i].transform.up * goUpDistance;

            Action callback = null;

            if (i == 0)
                callback = () => SmashItems(items);

            LeanTween.move(items[i].gameObject, targetPosition, goUpDuration)
                .setEase(goUpEasing).setOnComplete(callback);


        }
    }

    private void SmashItems ( List<Item> items)
    {

        for ( int i = 0; i< items.Count; i++ )
            Destroy(items[i].gameObject);

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

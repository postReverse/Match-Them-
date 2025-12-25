using System;
using UnityEngine;
using static UnityEditor.Progress;

public class ImputManager : MonoBehaviour
{

    public static Action<Item> ItemClicked;

    [Header("Settings")]
    [SerializeField] private Material outlineMaterial;
    private Item CurrentItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            HandleDrag();
        else if (Input.GetMouseButtonUp(0))
            HandleMouseUp();
    }

    private void HandleDrag()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100);

        if (hit.collider == null)
        {

            DeselectCurrentItem();
            return;
        }

        if (hit.collider.transform.parent == null )
        {
            return;
        }

        if (!hit.collider.transform.parent.TryGetComponent(out Item item))
        {
            DeselectCurrentItem();
            return;
       
        }

        DeselectCurrentItem();

        CurrentItem = item;
        CurrentItem.Select(outlineMaterial);
    }

    private void DeselectCurrentItem()
    {
        if (CurrentItem != null )
            CurrentItem.DeSelect();
        CurrentItem = null;
    }

    private void HandleMouseUp()
    {
        if(CurrentItem == null ) return;


        CurrentItem.DeSelect();
        ItemClicked?.Invoke(CurrentItem);
        CurrentItem = null; 
    }

}

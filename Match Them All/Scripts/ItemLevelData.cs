using UnityEngine;

[System.Serializable]
public struct ItemLevelData 
{

    public Item itemPrefab;
    public bool isGoal;

    [NaughtyAttributes.ValidateInput("ValidateAmount","Amount must be a multiple of 3.")]
    [NaughtyAttributes.AllowNesting]
    [Range(0,100)]
    public int Amount; 

    private bool ValidateAmount()
    {
        return Amount % 3 == 0;

    }

}

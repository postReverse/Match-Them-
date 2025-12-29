using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GoalManager : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Transform goalCardsParent;
    [SerializeField] private GoalCard goalCardPrefab;

    [Header("Data")]
    private ItemLevelData[] goals;
    private List<GoalCard> goalCards = new List<GoalCard>();    

    private void Awake()
    {
        LevelManager.levelSpawned += OnLevelSpawned;
        ItemSpotsManager.itemPickedUp += OnItemPickedUp;
    }

    private void OnDestroy()
    {
        LevelManager.levelSpawned -= OnLevelSpawned;
        ItemSpotsManager.itemPickedUp -= OnItemPickedUp;
    }

    private void OnLevelSpawned(Level level)
    {
        goals = level.GetGoals();

        GenerateGoalCards();

    }

    private void GenerateGoalCards()
    {

        for (int i = 0; i < goals.Length; i++)
            GenerateGoalCard(goals[i]);

    }

    private void GenerateGoalCard(ItemLevelData itemLevelGoal)
    {
        GoalCard cardInstance = Instantiate(goalCardPrefab,goalCardsParent);

        cardInstance.Configure(itemLevelGoal.Amount);

        goalCards.Add(cardInstance);

    }

    private void OnItemPickedUp(Item item)
    {
        for (int i = 0; i < goals.Length; i++) {

            if (!goals[i].itemPrefab.ItemName.Equals(item.ItemName))
                continue;

            goals[i].Amount--;

            if (goals[i].Amount <= 0)
                CompleteGoal(i);
            else
                goalCards[i].UpdateAmount(goals[i].Amount);
        
            break;
        
        }

    }

    private void CompleteGoal(int goalIndex)
    {
        Debug.Log("Goal Complete: " + goals[goalIndex].itemPrefab.ItemName);

        goalCards[goalIndex].Complete();

        CheckIfLevelIsComplete();

    }

    private void CheckIfLevelIsComplete()
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].Amount > 0)
                return;
        }
        Debug.Log("Level Complete");
    }
}

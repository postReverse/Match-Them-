using System;
using UnityEngine;


public class LevelManager : MonoBehaviour
{

    [Header("Datas")]
    [SerializeField] private Level[] levels;

    private const string levelKey = "Level";
    private int levelIndex;

    [Header("Settings")]
    private Level currentLevel;

    [Header("Action")]
    public static Action<Level> levelSpawned;

    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        levelIndex = PlayerPrefs.GetInt(levelKey);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(levelKey,levelIndex);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnLevel();
    }

    private void SpawnLevel()
    {

        transform.Clear();

        int validateLevelIndex = levelIndex % levels.Length;

        currentLevel = Instantiate(levels[validateLevelIndex], transform);

        levelSpawned?.Invoke(currentLevel);

    }
}

using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum SceneIndex
    {
        BootstrapScene,
        MainMenuScene,
        SortingInventoryScene,
        PackageDeliveryScene,
        UpgradeMenuScene
    }
    public static GameManager instance;

    [Header("Data for upgrades")]
    public List<InventoryConfigObject> inventoryConfigs;

    private TimeSystem timeSystem;

    private float money;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        timeSystem = GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>();

#if !UNITY_EDITOR
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
#endif
    }

    public void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int next_index = index + 1;
        switch ((SceneIndex)SceneManager.GetActiveScene().buildIndex)
        {
            case SceneIndex.BootstrapScene:
            case SceneIndex.MainMenuScene:
            case SceneIndex.SortingInventoryScene:
                SceneManager.LoadScene(next_index);
                break;
            case SceneIndex.PackageDeliveryScene:
                timeSystem.StopTime();
                SceneManager.LoadScene(next_index);
                break;
            case SceneIndex.UpgradeMenuScene:
                LoadNewDay();
                break;
            default:
                Debug.LogError("Unrecognized scene: " + SceneManager.GetActiveScene().buildIndex);
                break;
        };
    }

    public void RestartDay()
    {
        timeSystem.RestartDay();
        SceneManager.LoadScene((int)SceneIndex.SortingInventoryScene);
    }

    public void LoadNewDay()
    {
        timeSystem.StartNextDay();
        SceneManager.LoadScene((int)SceneIndex.SortingInventoryScene);
    }

    public void StartNewGame()
    {
        timeSystem.SetTime(0f);
        SaveSystem.DataManager.instance.ResetGameData();
        money = 0;
        RestartDay();
    }

    public void ContinueGame()
    {
        SaveSystem.DataManager.instance.LoadGame();
        SceneManager.LoadScene(SaveSystem.DataManager.instance.GetLastSceneIndex());
        timeSystem.StartTime();
    }

    public void RewardForDelivery(Package package)
    {
        money++;
    }
}

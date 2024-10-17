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

    [SerializeField] private BoolVariable isGamePaused;

    [Header("Data for upgrades")]
    public List<InventoryConfigObject> inventoryConfigs;

    private TimeSystem timeSystem;
    private Inventory inventory;

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
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        isGamePaused.value = false;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

#if !UNITY_EDITOR
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
#endif
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (timeSystem == null) return;

        switch ((SceneIndex)scene.buildIndex)
        {
            case SceneIndex.BootstrapScene:
            case SceneIndex.MainMenuScene:
            case SceneIndex.SortingInventoryScene:
            case SceneIndex.PackageDeliveryScene:
                timeSystem.StartTime();
                break;
            case SceneIndex.UpgradeMenuScene:
                timeSystem.StopTime();
                break;
            default:
                Debug.LogError("Unrecognized scene: " + SceneManager.GetActiveScene().buildIndex);
                break;
        };
    }

    public bool IsGamePaused()
    { 
        return isGamePaused.value;
    }

    public void PauseGame()
    {
        isGamePaused.value = true;
    }

    public void UnpauseGame()
    {
        isGamePaused.value = false;
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
            case SceneIndex.PackageDeliveryScene:
                SceneManager.LoadScene(next_index);
                break;
            case SceneIndex.UpgradeMenuScene:
                StartNextDay();
                break;
            default:
                Debug.LogError("Unrecognized scene: " + SceneManager.GetActiveScene().buildIndex);
                break;
        };
    }

    public void RestartDay()
    {
        timeSystem.ResetDay();
        inventory.Reset();
        SceneManager.LoadScene((int)SceneIndex.SortingInventoryScene);
    }

    public void StartNextDay()
    {
        timeSystem.SetNextDay();
        inventory.Reset();
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
    }

    public void RewardForDelivery(Package package)
    {
        money++;
    }

    public float GetMoney()
    {
        return money;
    }
}

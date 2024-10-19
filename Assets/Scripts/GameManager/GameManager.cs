using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public enum GameScene
    {
        BootstrapScene,
        MainMenuScene,
        SortingInventoryScene,
        PackageDeliveryScene,
        UpgradeMenuScene
    }
    public static GameManager instance;

    [SerializeField] private BoolVariable isGamePaused;
    [SerializeField] private int money;
    [SerializeField] private int baseMinutesPerMovement = 20;
    [SerializeField] private int baseMinutesPerInteraction = 5;

    [Header("Data for upgrades")]
    public List<InventoryConfigObject> inventoryConfigs;
    public int inventoryConfigIndex = 0;
    public float speedMultiplier = 1f;
    public float packageValueMultiplier = 1f;

    [Header("Data for plot")]
    public NPCCollection npcCollection;
    public RandomGameObjectGenerator packageIconGen;

    private TimeSystem timeSystem;
    private Inventory inventory;

    public void Save(GameData gameData)
    {
        var data = gameData.gameManagerData;
        ISaveable.AddKey(data, "money", money);
        ISaveable.AddKey(data, "inventoryConfigIndex", inventoryConfigIndex);
        ISaveable.AddKey(data, "speedMultiplier", speedMultiplier);
        ISaveable.AddKey(data, "packageValueMultiplier", packageValueMultiplier);
    }

    public bool Load(GameData gameData)
    {
        foreach (var key_value in gameData.gameManagerData)
        {
            var parsed = ISaveable.ParseKey(key_value);
            string key = parsed[0];
            string value = parsed[1];
            //Debug.Log("Loading key: " + key + " value: " + value);
            switch (key)
            {
                case "money":
                    money = Convert.ToInt32(value);
                    break;
                case "inventoryConfigIndex":
                    inventoryConfigIndex = Convert.ToInt32(value);
                    break;
                case "speedMultiplier":
                    speedMultiplier = (float)Convert.ToDouble(value);
                    break;
                case "packageValueMultiplier":
                    packageValueMultiplier = (float)Convert.ToDouble(value);
                    break;
                default:
                    Debug.LogError("Unrecognized key: " + key + " value: " + value);
                    break;
            }
        }

        return true;
    }

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

        switch ((GameScene)scene.buildIndex)
        {
            case GameScene.BootstrapScene:
            case GameScene.MainMenuScene:
                break;
            case GameScene.SortingInventoryScene:
            case GameScene.PackageDeliveryScene:
                timeSystem.StartTime();
                break;
            case GameScene.UpgradeMenuScene:
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

    public void LoadScene(GameScene sceneIndex)
    {
        // TODO: Reorganize scene loading
        SceneManager.LoadScene((int)sceneIndex);
    }

    public void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        int next_index = index + 1;
        switch ((GameScene)SceneManager.GetActiveScene().buildIndex)
        {
            case GameScene.BootstrapScene:
            case GameScene.MainMenuScene:
            case GameScene.SortingInventoryScene:
            case GameScene.PackageDeliveryScene:
                SceneManager.LoadScene(next_index);
                break;
            case GameScene.UpgradeMenuScene:
                StartNextDay();
                break;
            default:
                Debug.LogError("Unrecognized scene: " + SceneManager.GetActiveScene().buildIndex);
                break;
        };
    }

    public void SpendMovementTime()
    {
        float baseTime = baseMinutesPerMovement / 60f;
        float moveTime = baseTime / speedMultiplier;
        timeSystem.AdvanceTime(moveTime);
    }

    public void SpendInteractionTime()
    {
        float baseTime = baseMinutesPerInteraction / 60f;
        float inteTime = baseTime / speedMultiplier;
        timeSystem.AdvanceTime(inteTime);
    }

    public void RestartDay()
    {
        timeSystem.ResetDay();
        inventory.Reset();
        SceneManager.LoadScene((int)GameScene.SortingInventoryScene);
    }

    public void StartNextDay()
    {
        timeSystem.SetNextDay();
        inventory.Reset();
        SceneManager.LoadScene((int)GameScene.SortingInventoryScene);
    }

    public void StartNewGame()
    {
        timeSystem.SetTime(0f);
        SaveSystem.DataManager.instance.ResetGameData();
        money = 0;
        inventoryConfigIndex = 0;
        speedMultiplier = 1.0f;
        packageValueMultiplier = 1.0f;
        RestartDay();
    }

    public void ContinueGame()
    {
        SaveSystem.DataManager.instance.LoadGame();
        SceneManager.LoadScene(SaveSystem.DataManager.instance.GetLastSceneIndex());
    }

    public void RewardForDelivery(Package package)
    {
        money += package.cost;
    }

    public int GetMoney()
    {
        return money;
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount)
        {
            return false;
        }
        money -= amount;
        return true;
    }

    public InventoryConfigObject GetInventoryConfig()
    {
        return inventoryConfigs[inventoryConfigIndex];
    }

    public InventoryConfigObject GetInventoryConfig(int index)
    {
        return ((index >= 0) && (index < inventoryConfigs.Count)) ? inventoryConfigs[index] : null;
    }
}

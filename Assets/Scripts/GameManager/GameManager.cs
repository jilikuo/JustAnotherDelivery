using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    [Header("Menus")]
    InGameMenu inGameMenu;
    SettingsMenu settingsMenu;

    private List<GameObject> escMenuStack = new List<GameObject>();

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
        inGameMenu = GameObject.FindGameObjectWithTag("InGameMenu").GetComponent<InGameMenu>();
        settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu").GetComponent<SettingsMenu>();
        ShowSettingsMenu(false);
        timeSystem = GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>();
        timeSystem.StopTime();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        isGamePaused.value = false;
        InitGameData();

#if !UNITY_EDITOR
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!CloseLastEscMenu())
            {
                MaybeShowInGameMenu(true);
            }
        }
    }

    public void AddEscMenu(GameObject menu)
    {
        escMenuStack.Add(menu);
    }

    public void RemoveEscMenu(GameObject menu)
    {
        escMenuStack.Remove(menu);
    }

    private bool CloseLastEscMenu()
    {
        while ((escMenuStack.Count > 0) && (!escMenuStack.Last().activeSelf))
        {
            escMenuStack.Remove(escMenuStack.Last());
        }

        if (escMenuStack.Count > 0)
        {
            // Close the last escape menu
            var menu = escMenuStack.Last();
            menu.SetActive(false);
            escMenuStack.Remove(menu);
            return true;
        }

        if (inGameMenu.IsVisible())
        {
            inGameMenu.Show(false);
            return true;
        }

        return false;
    }

    public void MaybeShowInGameMenu(bool show = true)
    {
        if (!show)
        {
            // Always close
            inGameMenu.Show(false);
            return;
        }

        // Sometimes open
        switch ((GameScene)SceneManager.GetActiveScene().buildIndex)
        {
            case GameScene.BootstrapScene:
            case GameScene.MainMenuScene:
                break;
            case GameScene.SortingInventoryScene:
            case GameScene.PackageDeliveryScene:
            case GameScene.UpgradeMenuScene:
                inGameMenu.Show(true);
                break;
            default:
                Debug.LogError("Unrecognized scene: " + SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }

    public void ShowSettingsMenu(bool show = true)
    {
        settingsMenu.Show(show);
    }

    public bool IsGamePaused()
    { 
        return isGamePaused.value;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused.value = true;
    }

    public void UnpauseGame()
    {
        isGamePaused.value = false;
        Time.timeScale = 1;
    }

    public void LoadScene(GameScene gameScene)
    {
        if (timeSystem != null)
        {
            timeSystem.StopTime();

            if (gameScene == GameScene.MainMenuScene)
            {
                EndGame();
            }
        }

        SceneManager.LoadScene((int)gameScene);

        if (timeSystem != null)
        {
            switch (gameScene)
            {
                case GameScene.BootstrapScene:
                case GameScene.MainMenuScene:
                    break;
                case GameScene.SortingInventoryScene:
                case GameScene.PackageDeliveryScene:
                    timeSystem.StartTime();
                    break;
                case GameScene.UpgradeMenuScene:
                    break;
                default:
                    Debug.LogError("Unrecognized scene: " + SceneManager.GetActiveScene().buildIndex);
                    break;
            }
        }
    }

    public void LoadNextScene()
    {
        GameScene gameScene = (GameScene)SceneManager.GetActiveScene().buildIndex;
        switch (gameScene)
        {
            case GameScene.BootstrapScene:
            case GameScene.MainMenuScene:
            case GameScene.SortingInventoryScene:
            case GameScene.PackageDeliveryScene:
                LoadScene(gameScene + 1);
                break;
            case GameScene.UpgradeMenuScene:
                StartNextDay();
                break;
            default:
                Debug.LogError("Unrecognized scene: " + SceneManager.GetActiveScene().buildIndex);
                break;
        }
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

    public void InitGameData()
    {
        timeSystem.SetTime(0f);
        money = 0;
        inventoryConfigIndex = 0;
        speedMultiplier = 1.0f;
        packageValueMultiplier = 1.0f;
    }

    public void ResetGameData()
    {
        SaveSystem.DataManager.instance.ResetGameData();
        InitGameData();
    }

    public void RestartDay()
    {
        SaveSystem.DataManager.instance.LoadGame();
        timeSystem.ResetDay();
        inventory.Reset();
        LoadScene(GameScene.SortingInventoryScene);
    }

    public void StartNextDay()
    {
        timeSystem.SetNextDay();
        inventory.Reset();
        LoadScene(GameScene.SortingInventoryScene);
    }

    public void StartNewGame()
    {
        ResetGameData();
        RestartDay();
    }

    public void ContinueGame()
    {
        SaveSystem.DataManager.instance.LoadGame();
        LoadScene((GameScene)SaveSystem.DataManager.instance.GetLastSceneIndex());
    }

    public void EndGame()
    {
        switch ((GameScene)SceneManager.GetActiveScene().buildIndex)
        {
            case GameScene.BootstrapScene:
            case GameScene.MainMenuScene:
            case GameScene.SortingInventoryScene:
            case GameScene.PackageDeliveryScene:
                break;
            case GameScene.UpgradeMenuScene:
                SaveSystem.DataManager.instance.UpdateAndSaveToFile();
                break;
            default:
                Debug.LogError("Unrecognized scene: " + SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }

    public void ExitGame()
    {
        EndGame();
        Application.Quit();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private string mainMenuScene = "MainMenuScene";
    [SerializeField] private string inventorySortingScene = "InventorySortingScene";
    [SerializeField] private string packageDeliveryScene = "PackageDeliveryScene";
    [SerializeField] private string upgradeMenuScene = "UpgradeMenuScene";

    private TimeSystem timeSystem;
    private int lastSceneIndex;

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
        lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

#if !UNITY_EDITOR
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(mainMenuScene);
        }
#endif
    }

    public void LoadNextScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == inventorySortingScene)
        {
            SceneManager.LoadScene(packageDeliveryScene);
        }
        else if (currentSceneName == packageDeliveryScene)
        {
            timeSystem.StopTime();
            SceneManager.LoadScene(upgradeMenuScene);
        }
        else if (currentSceneName == upgradeMenuScene)
        {
            LoadNewDay();
        }
    }

    public void RestartDay()
    {
        timeSystem.RestartDay();
        SceneManager.LoadScene(inventorySortingScene);
    }

    public void LoadNewDay()
    {
        timeSystem.StartNextDay();
        SceneManager.LoadScene(inventorySortingScene);
    }

    public void StartNewGame()
    {
        timeSystem.SetTime(0f);
        RestartDay();
    }

    public void ContinueGame()
    {
        // TODO: Add implementation
    }
}

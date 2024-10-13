using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string inventoryScene = "InventorySortingScene";
    public string mainMenuScene = "MainMenuScene";
    public string packageScene = "PackageSortingScene";
    public string upgradeScene = "UpgradeMenuScene";
    
    private float dayStartHour;
    private float dayEndHour;

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
        dayStartHour = timeSystem.dayStartHour;
        dayEndHour = timeSystem.dayEndHour;

        lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

#if !UNITY_EDITOR
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadNextScene();
        }
#endif
    }

    private void Update()
    {
        if (timeSystem.isTimeStopped.value &&
            timeSystem.currentTime.GetTime() >= dayEndHour && 
            (SceneManager.GetActiveScene().buildIndex != lastSceneIndex))
        {
            Debug.Log("Ran out of time");
            LoadUpgradeMenu();
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //if the scene is the second to last, the day ends earlier, time should be stopped.
        if (currentSceneIndex == (lastSceneIndex - 1))
        {
            timeSystem.StopTime();
            Debug.Log("Day ended before time ran out");
        }
        
        if (currentSceneIndex < (lastSceneIndex))
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            LoadNewDay();
        }
    }

    public void LoadUpgradeMenu()
    {
        SceneManager.LoadScene(upgradeScene);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadNewDay()
    {
        SceneManager.LoadScene(inventoryScene);
        timeSystem.StartNextDay();
        timeSystem.StopTimeAt(timeSystem.currentTime.GetTime() + (dayEndHour - dayStartHour));
        timeSystem.StartTime();
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(inventoryScene);
        timeSystem.SetTime(dayStartHour);
        timeSystem.StartTime();
        timeSystem.StopTimeAt(dayEndHour);
    }

    public void ContinueGame()
    {
        // TODO: Add implementation
    }
}

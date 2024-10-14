using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveSystem;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data.Common;

namespace SaveSystem
{
    public class DataManager : MonoBehaviour
    {
        private GameData gameData = new GameData();

        [SerializeField] private string autoSaveDirectory;
        [SerializeField] private string autoSaveFilename = "autosave.dat";
        [SerializeField] private int minSceneSaveIndex = 2;

        private FileManager autoSaveFileManager;

        public static DataManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                gameData = null;
                autoSaveFileManager = new FileManager(autoSaveDirectory, autoSaveFilename);
                LoadFromFile();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            SceneManager.activeSceneChanged += ChangedActiveScene;
        }

        private void ChangedActiveScene(Scene current, Scene next)
        {
            UpdateAndSaveToFile();
        }

        public bool HasLoadedGameData()
        {
            Debug.Log("GameData loaded: " + (gameData != null));
            if (gameData != null) Debug.Log("GameData Scene name: " + gameData.sceneName + " index: " + gameData.sceneIndex);
            return (gameData != null) && (gameData.sceneIndex >= minSceneSaveIndex);
        }

        public void LoadGame()
        {
            foreach (var saveable in FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray())
            {
                saveable.Load(gameData);
            }
        }

        public int GetLastSceneIndex()
        {
            return gameData.sceneIndex;
        }

        public void UpdateAndSaveToFile()
        {
            if (UpdateGameData())
            {
                SaveToFile();
            }
        }

        public void SaveToFile()
        {
            if ((gameData != null) && (autoSaveFileManager != null))
            { 
                autoSaveFileManager.Save(gameData);
            }
        }

        public void LoadFromFile()
        {
            gameData = autoSaveFileManager.Load();
        }

        public void ResetGameData()
        {
            gameData = new GameData();
        }

        private bool UpdateGameData()
        {
            if (SceneManager.GetActiveScene().buildIndex < minSceneSaveIndex)
            {
                return false;
            }
            IEnumerable<ISaveable> saveableObjs = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
            List<string> destroyedObjectIds = ((gameData != null) && (gameData.destroyedObjectIds != null)) ? gameData.destroyedObjectIds : new List<string>();
            gameData = new GameData();
            gameData.sceneName = SceneManager.GetActiveScene().name;
            gameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            foreach (var obj in saveableObjs)
            {
                obj.Save(gameData);
            }
            gameData.destroyedObjectIds = destroyedObjectIds;
            return true;
        }

        public void AddDestroyedDestroyable(IDestroyable destroyable)
        {
            gameData.destroyedObjectIds.Add(destroyable.GenerateDestroyedId());
        }

        public bool IsDestroyedDestroyable(IDestroyable destroyable)
        {
            return (gameData != null) && (gameData.destroyedObjectIds.Contains(destroyable.GenerateDestroyedId()));
        }

        public class FileManager
        {
            private string filePath;
            public FileManager(string directory, string fileName)
            {
                filePath = System.IO.Path.Combine(directory, fileName);
                //Debug.Log("Autosave relative path: " + filePath);
                filePath = System.IO.Path.GetFullPath(filePath);
                //Debug.Log("Autosave absolute path: " + filePath);
            }
            public bool Save(GameData gameData)
            {
                //Debug.Log("Saving game to file: " + filePath);
                string data;
                try
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            data = JsonUtility.ToJson(gameData);
                            writer.Write(data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Failed to save game data: ERROR: " + ex.ToString());
                    return false;

                }
                return true;
            }

            public GameData Load()
            {
                //Debug.Log("Loading game from file: " + filePath);
                GameData gameData = null;
                string data;
                try
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
                    using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        using (StreamReader reader = new StreamReader(stream))

                        data = reader.ReadToEnd();
                        gameData = JsonUtility.FromJson<GameData>(data);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("Failed to load game data: ERROR: " + ex.ToString());
                }
                return gameData;
            }
        }
    }
}

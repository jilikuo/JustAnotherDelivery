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

        [SerializeField]
        private string autoSaveDirectory;
        [SerializeField]
        private string autoSaveFilename = "autosave.dat";
        [SerializeField]
        private int updatesPerSave = 600;

        private FileManager autoSaveFileManager;

        private int updateCounter = 2; // Ensure everything is updated, before first save

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

        // Update is called once per frame
        void Update()
        {
            if (--updateCounter > 0)
            {
                return;
            }
            updateCounter = updatesPerSave;
            UpdateAndSaveToFile();
        }
        private void OnApplicationQuit()
        {
            SaveToFile();
        }

        public bool HasLoadedGameData()
        {
            return (gameData != null) && (gameData.sceneIndex >= 0);
        }

        public bool Load(ISaveable saveable)
        {
            //Debug.Log("Loading class: " + saveable.GetType().Name);
            //Debug.Log("instance.HasCurrentSceneData(): " + instance.HasCurrentSceneData());
            if (!instance.HasLoadedGameData())
            {
                //Debug.Log("Loading disabled for class: " + saveable.GetType().Name);
                return false;
            }

            return saveable.Load(gameData);
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
            IEnumerable<ISaveable> saveableObjs = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
            if (!saveableObjs.Any())
            {
                return false;
            }
            if (gameData == null) gameData = new GameData();
            gameData.sceneName = SceneManager.GetActiveScene().name;
            gameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            foreach (var obj in saveableObjs)
            {
                obj.Save(gameData);
            }
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

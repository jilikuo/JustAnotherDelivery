using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace SaveSystem
{
    /*
     * How to add data to save system:
     * 1. Add a List<string> for your data, named for your class.
     * 2. Extend and implement ISaveable, to write-to(Save)/read-from(Load) your data.
     *    * Save will be called automatically
     *    * Call SaveSystem.DataManager.instance.Load(this), to load data from file, if specified
     *      * Load should probably be in Awake or Start
     *    * Data may be stored in a List<string> (Dictionaries cannot be converted to json)
     *      * AddKey and ParseKey have been provided to pack/unpack the data.
     */
    [System.Serializable]
    public class GameData
    {
        public int sceneIndex = -1;
        public string sceneName;

        // Destroyed objects
        public List<string> destroyedObjectIds = new List<string>();
    }

    public interface ISaveable
    {
        // Copy data to gameData
        public void Save(GameData gameData);

        // Copy data from gameData
        // * Return false, if data does not exist
        public bool Load(GameData gameData);

        // Utility for packaging data for GameData lists
        static public void AddKey<T>(List<string> values, string key, T value)
        {
            var key_value = string.Join(":", key, value);
            values.Add(key_value);
            //Debugger.Log("Added key: " + key_value);
        }
        static public void AddKey<T>(List<string> values, Vector3Int key, T value)
        {
            var key_value = string.Join(":", Vector3IntToString(key), value);
            values.Add(key_value);
            //Debugger.Log("Added key: " + key_value);
        }

        // Utility for unpacking data from GameData lists
        static public string[] ParseKey(string key_value)
        {
            //Debugger.Log("Parsed key: " + key_value);
            return key_value.Split(':', 2);
        }

        static public Vector3Int Vector3IntFromString(string str)
        {
            var tmp = str.Split(',');
            return new Vector3Int(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]));
        }

        static public string Vector3IntToString(Vector3Int v)
        {
            return string.Join(',', v.x, v.y, v.z);
        }
    }
    public interface IDestroyable
    {
        // Create an unique ID for the destroyed objects list
        public string GenerateDestroyedId();

        static public string GetGameObjectPath(GameObject gameObject)
        {

            string name = gameObject.name;
            while (gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;
                name = gameObject.name + "/" + name;
            }
            return name;
        }

        static public string GetGameObjectPathWId(GameObject gameObject, string id)
        {
            return string.Join(':', GetGameObjectPath(gameObject), id);
        }
    }
}

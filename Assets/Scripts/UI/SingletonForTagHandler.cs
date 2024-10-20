using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem;
using System.Linq;

public class SingletonForTagHandler : MonoBehaviour
{
    private void Awake()
    {
        if (gameObject.tag != null)
        {
            var objs = GameObject.FindGameObjectsWithTag(gameObject.tag);

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Debug.LogError("Failed to determine tag");
        }
    }
}

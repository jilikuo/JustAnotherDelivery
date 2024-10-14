using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    public static UICamera instance;
    public Transform playerTransform;

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

    private void Update()
    {
        MoveCamera();
    }

    public void MoveCamera()
    {
        if (this.transform.position != playerTransform.position)
        {
            this.transform.position = playerTransform.position;
        }
        if (this.transform.rotation != playerTransform.rotation)
        {
            this.transform.rotation = playerTransform.rotation;
        }
    }
}

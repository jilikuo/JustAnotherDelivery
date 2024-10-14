using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICamera : MonoBehaviour
{
    public static UICamera instance;
    public GameObject player;

    public bool isSet = false;

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
        if (SceneManager.GetActiveScene().name == "PackageDeliveryScene" && !isSet)
        {
            SetCamera();
        }

        if (isSet && SceneManager.GetActiveScene().name != "PackageDeliveryScene")
        {
            isSet = false;
        }

        MoveCamera();
    }

    public void MoveCamera()
    {
        if (this.transform.position != player.transform.position)
        {
            this.transform.position = player.transform.position;
        }
        if (this.transform.rotation != player.transform.rotation)
        {
            this.transform.rotation = player.transform.rotation;
        }
    }

    private void SetCamera()
    {
        player.GetComponent<MovementScript>().RealignToWaypoint();
        this.transform.position = player.transform.position;
        this.transform.rotation = player.transform.rotation;
        isSet = true;
    }
}

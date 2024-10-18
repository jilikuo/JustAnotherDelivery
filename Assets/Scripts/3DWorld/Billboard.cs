using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        // Billboard effect, the object will always face the camera, but only in the X and Z axis
        this.transform.LookAt(_camera.transform.position);
        this.transform.rotation = Quaternion.Euler(90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
    }
}

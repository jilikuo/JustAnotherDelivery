using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapAvatar : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject avatarObject;
    public float horizontalFactor = -2.7f; // - means the world coordinates are opposite to UI coordinates, 2 is the relation between world space and UI space
    public float verticalFactor = -2.875f;

    public int rotationFactor = -1;
    public int rotationDifference = 90; // 90 degrees difference between world rotation and UI rotation

    private Transform playerTransform;
    private RectTransform avatarRectTransform;
    private bool playerMoved = false;

    private void Start()
    {
        #region NotNullChecks
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
            {
                throw new System.Exception("Minimap Avatar needs a player object to represent");
            }
        }

        if (avatarObject == null)
        {
            avatarObject = GameObject.FindGameObjectWithTag("MinimapAvatar");
            if (avatarObject == null)
            {
                throw new System.Exception("Minimap Avatar needs an avatar object to move");
            }
        }
        #endregion

        playerTransform = playerObject.transform;
        avatarRectTransform = avatarObject.GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (playerMoved)
        {
            MovePlayerAvatar();
            playerMoved = false;
        }
    }

    public void RegisterPlayerMovement()
    {
        playerMoved = true;
    }

    private void MovePlayerAvatar()
    {
        Vector2 targetPos = new(playerTransform.position.x * horizontalFactor, playerTransform.position.z * verticalFactor);
        avatarRectTransform.anchoredPosition = targetPos;
        avatarRectTransform.rotation = Quaternion.Euler(0, 0, (playerTransform.eulerAngles.y + rotationDifference) * rotationFactor);
    }
}

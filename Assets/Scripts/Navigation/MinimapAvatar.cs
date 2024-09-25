using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapAvatar : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject avatarObject;
    public int stepSize = 10;

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
        Vector2 targetPos = new(playerTransform.position.x * stepSize, playerTransform.position.y * stepSize);
        avatarRectTransform.anchoredPosition = targetPos;
    }
}

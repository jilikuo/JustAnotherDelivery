using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapAvatar : MonoBehaviour
{ 
    [SerializeField] private RectTransform avatarRectTransform;
    private MovementScript movementScript;

    private void Start()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            throw new System.Exception("No object with the tag 'Player' was found");
        }

        movementScript = playerObject.GetComponent<MovementScript>();
        if (movementScript == null)
        {
            throw new System.Exception("Player object has no component of the type 'MovementScript'");
        }
        UpdateMinimapAvatar();
    }
    public void UpdateMinimapAvatar()
    {
        avatarRectTransform.anchoredPosition = new Vector2( movementScript.GetCurrentWaypoint().minimapPosition.x, 
                                                            movementScript.GetCurrentWaypoint().minimapPosition.y);

        avatarRectTransform.localRotation = Quaternion.Euler(0, 0, movementScript.GetCurrentWaypoint().minimapRotation.z);
    }
}

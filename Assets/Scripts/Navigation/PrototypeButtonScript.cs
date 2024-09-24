using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeButtonScript : MonoBehaviour
{
    public Direction movementDirection;
    public GameObject playerObject;

    private PrototypeMovementScript movementScript;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            throw new System.Exception("PrototypeButtonScript needs a player object to move");
        }

        movementScript = playerObject.GetComponent<PrototypeMovementScript>();

        if (movementScript == null)
        {
            throw new System.Exception("PrototypeButtonScript needs a movement script to move");
        }
    }

    public void MovePlayer()
    {
        movementScript.MovePlayer(movementDirection);
    }
}

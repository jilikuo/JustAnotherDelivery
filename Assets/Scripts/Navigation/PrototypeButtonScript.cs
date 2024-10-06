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
            throw new System.Exception("PrototypeButtonScript needs a player object to move. No object with the tag 'Player' was found");
        }

        movementScript = playerObject.GetComponent<PrototypeMovementScript>();
        if (movementScript == null)
        {
            throw new System.Exception("PrototypeButtonScript needs a movement script to move. Player object has no component of the type 'PrototypeMovementScript'");
        }
    }

    public void MovePlayer()
    {
        movementScript.MovePlayer(movementDirection);
    }
}

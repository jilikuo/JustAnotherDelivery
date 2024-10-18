using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavButtonScript : MonoBehaviour
{
    public Direction movementDirection;
    public GameObject playerObject;

    private MovementScript movementScript;
    private bool isDirectionValid;
    private bool isDirectionAccessible;
    public bool isThisUI = true;

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            throw new System.Exception("ButtonScript needs a player object to move. No object with the tag 'Player' was found");
        }

        movementScript = playerObject.GetComponent<MovementScript>();
        if (movementScript == null)
        {
            throw new System.Exception("ButtonScript needs a movement script to move. Player object has no component of the type 'MovementScript'");
        }

        if (isThisUI)
        {
            CheckCanMove();
        }
    }

    public void MovePlayer()
    {
        movementScript.MovePlayer(movementDirection);
        Debug.Log("Player moved " + movementDirection);
    }

    public void CheckCanMove()
    {
        isDirectionValid = movementScript.ValidateDirection(movementDirection);
        isDirectionAccessible = movementScript.CheckDirection(movementDirection);

        this.gameObject.GetComponent<UnityEngine.UI.Button>().interactable = (isDirectionValid && isDirectionAccessible);

        // Debug.Log("Direction " + movementDirection + " is valid = " + isDirectionValid + " and accessible = " + isDirectionAccessible);
    }
}

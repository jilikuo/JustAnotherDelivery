using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class KeyboardMovementHandler : MonoBehaviour
{
    private GameObject playerObject;
    private MovementScript movementScript;
    private NavigationMenuHandler navigationManager;
    private MinimapAvatar minimapAvatar;
    private NavButtonScript navButtonScript;
    private Direction direction;
    private bool willTryMove = false;

    private void Start()
    {
        #region NullChecks
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            throw new System.Exception("No object with the tag 'Player' was found");
        }

        movementScript = playerObject.GetComponent<MovementScript>();
        if (movementScript == null)
        {
            throw new System.Exception("Player object has no component of the type 'MovementScript'");
        }

        navigationManager = GameObject.FindGameObjectWithTag("NavigationManager").GetComponent<NavigationMenuHandler>();
        if (navigationManager == null)
        {
            throw new System.Exception("Failed to locate NavigationMenuHandler");
        }

        minimapAvatar = GameObject.FindGameObjectWithTag("MinimapManager").GetComponent<MinimapAvatar>();
        if (minimapAvatar == null)
        {
            throw new System.Exception("Failed to locate MinimapAvatar");
        }

        navButtonScript = this.gameObject.GetComponent<NavButtonScript>();
        if (navButtonScript == null)
        {
            throw new System.Exception("Failed to locate NavButtonScript");
        }
        #endregion
    }

    private void Update()
    {
        if (GameManager.instance.IsTimeStopped()) return;

        ReadInputs();

        if (willTryMove)
        {
            willTryMove = false;
            if (CheckCanMove(direction))
            {
                MovePlayer(direction);
            }
        }
    }

    private void ReadInputs()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Direction.North;
            willTryMove = true;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Direction.South;
            willTryMove = true;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Direction.West;
            willTryMove = true;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Direction.East;
            willTryMove = true;
        }
    }

    private void MovePlayer(Direction direction)
    {
        navButtonScript.movementDirection = direction;
        navButtonScript.MovePlayer();
        navigationManager.UpdateNavMenuAfterMovement();
        minimapAvatar.UpdateMinimapAvatar();
    }

    private bool CheckCanMove(Direction direction)
    {
        if (navigationManager.IsPlayerInteracting())
        {
            return false;
        }

        if (!movementScript.ValidateDirection(direction) ||
            !movementScript.CheckDirection(direction))
        {
            return false;
        }

        return true;
    }
}

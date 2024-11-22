using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    South,
    West,
    East
}

public class MovementScript : MonoBehaviour
{
    public Waypoints hqWaypoint;
    public AudioSource walkingSound;

    private GameObject playerObject;
    private Transform playerTransform;
    private Transform cameraTransform;
    private Waypoints currentWaypoint;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            playerObject = this.gameObject;
        }

        playerTransform = playerObject.transform;

        if (currentWaypoint == null)
        {
            if (hqWaypoint == null)
            {
                throw new System.Exception("No HQ Waypoint assigned to MovementScript.");
            }
            currentWaypoint = hqWaypoint;
        }

        playerTransform.position = new Vector3(currentWaypoint.position.x,
                                               currentWaypoint.position.y,
                                               currentWaypoint.position.z);

        if (walkingSound == null)
        {
            walkingSound = playerObject.GetComponentInChildren<AudioSource>();
            if (walkingSound == null)
            {
                Debug.LogError("No AudioSource found in Player GameObject childs.");
            }
        }
    }

    public void MovePlayer(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                currentWaypoint = currentWaypoint.northWaypoint;
                break;
            case Direction.South:
                currentWaypoint = currentWaypoint.southWaypoint;
                break;
            case Direction.West:
                currentWaypoint = currentWaypoint.westWaypoint;
                break;
            case Direction.East:
                currentWaypoint = currentWaypoint.eastWaypoint;
                break;
            default:
                throw new System.ArgumentException("Invalid Direction received at MovePlayer method");
        }

        if (!walkingSound.isPlaying)
        {
            walkingSound.Play();
        }

        playerTransform.position = new Vector3(currentWaypoint.position.x,
                                               currentWaypoint.position.y,
                                               currentWaypoint.position.z);

        playerTransform.rotation = Quaternion.Euler(currentWaypoint.rotation.x,
                                                    currentWaypoint.rotation.y,
                                                    currentWaypoint.rotation.z);

        GameManager.instance.SpendMovementTime();
    }

    public bool ValidateDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return currentWaypoint.northWaypoint.isValid;
            case Direction.South:
                return currentWaypoint.southWaypoint.isValid;
            case Direction.West:
                return currentWaypoint.westWaypoint.isValid;
            case Direction.East:
                return currentWaypoint.eastWaypoint.isValid;
            default:
                throw new System.ArgumentException("Invalid Direction received at ValidateDirection method");
        }
    }

    public bool CheckDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return currentWaypoint.northWaypoint.isAccessible;
            case Direction.South:
                return currentWaypoint.southWaypoint.isAccessible;
            case Direction.West:
                return currentWaypoint.westWaypoint.isAccessible;
            case Direction.East:
                return currentWaypoint.eastWaypoint.isAccessible;
            default:
                throw new System.ArgumentException("Invalid Direction received at CheckDirection method");
        }
    }

    public void RealignToWaypoint()
    {
        playerTransform.position = new Vector3(currentWaypoint.position.x,
                                       currentWaypoint.position.y,
                                       currentWaypoint.position.z);

        playerTransform.rotation = Quaternion.Euler(currentWaypoint.rotation.x,
                                                    currentWaypoint.rotation.y,
                                                    currentWaypoint.rotation.z);
    }

    public Waypoints GetCurrentWaypoint()
    {
        return currentWaypoint;
    }

    public void RestartWaypoint()
    {
        currentWaypoint = hqWaypoint;
        RealignToWaypoint();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward,
    Backward,
    Left,
    Right
}

public class PrototypeMovementScript : MonoBehaviour
{
    public GameObject playerObject;
    private Transform playerTransform;

    void Start()
    {
        #region NotNullChecks
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null)
            {
                throw new System.Exception("Placeholder Movement Script needs a player object to move");
            }
        }

        #endregion
        playerTransform = playerObject.transform;
    }

    public void MovePlayer(Direction direction)
    {
        playerTransform.position += direction switch
        {
            Direction.Forward => new Vector3(1, 0, 0),
            Direction.Backward => new Vector3(-1, 0, 0),
            Direction.Left => new Vector3(0, 1, 0),
            Direction.Right => new Vector3(0, -1, 0),
            _ => throw new System.ArgumentException("Invalid Direction received at MovePlayer method"),
        };
    }
}
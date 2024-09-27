using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class NavigationMenuHandler : MonoBehaviour
{
    public GameObject movementBar;
    public GameObject moveAheadButton;
    public GameObject moveBackButton;
    public GameObject moveLeftButton;
    public GameObject moveRightButton;

    public GameObject interactionBar;
    public GameObject nameLabel;
    public GameObject titleLabel;
    public GameObject messageBox;

    public GameObject emergencyButton;
    public GameObject personalAnswerButton;
    public GameObject professionalAnswerButton;
    public GameObject doNotSpeakButton;
    public GameObject fleeButton;

    private GameObject activeBar;

    private bool canCallEmergency = true;
    private bool canFlee = true;
    private bool interactionBarIsDirty = false;

    private void Awake()
    {
        #region Null Checks
        if (movementBar == null)
        {
            throw new System.Exception("Movement Bar not set in NavigationMenuHandler");
        }

        if (interactionBar == null)
        {
            throw new System.Exception("Interaction Bar not set in NavigationMenuHandler");
        }

        if (moveAheadButton == null || moveBackButton == null || moveLeftButton == null || moveRightButton == null)
        {
            throw new System.Exception("One or more movement buttons not set in NavigationMenuHandler");
        }

        if (nameLabel == null)
        {
            throw new System.Exception("Name label not set in NavigationMenuHandler");
        }

        if (titleLabel == null)
        {
            throw new System.Exception("Title label not set in NavigationMenuHandler");
        }

        if (messageBox == null)
        {
            throw new System.Exception("Message box not set in NavigationMenuHandler");
        }

        if (emergencyButton == null || personalAnswerButton == null || professionalAnswerButton == null || doNotSpeakButton == null || fleeButton == null)
        {
            throw new System.Exception("One or more interaction buttons not set in NavigationMenuHandler");
        }
        #endregion

        movementBar.SetActive(true);
        activeBar = movementBar;

        interactionBar.SetActive(false);
    }

    private void Update()
    {
        if (activeBar == interactionBar && interactionBarIsDirty)
        {
            UpdateInteractionButton();
        }
    }

    public void ActivateMovementBar()
    {
        activeBar.SetActive(false);
        movementBar.SetActive(true);
        activeBar = movementBar;
    }

    public void ActivateInteractionBar()
    {
        activeBar.SetActive(false);
        interactionBar.SetActive(true);
        activeBar = interactionBar;

        //TODO: Only make interaction bar dirty if canCallEmergency or CanFlee has changed
        interactionBarIsDirty = true;
    }

    public void SetActiveCharacterName(string name)
    {
        nameLabel.GetComponent<TMPro.TextMeshProUGUI>().text = name;
    }

    public void SetActiveCharacterTitle(string title)
    {
        titleLabel.GetComponent<TMPro.TextMeshProUGUI>().text = title;
    }

    public void SetMessage(string message)
    {
        messageBox.GetComponent<TMPro.TextMeshProUGUI>().text = message;
    }

    public void BlockMovement(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                moveAheadButton.SetActive(false);
                break;
            case Direction.Backward:
                moveBackButton.SetActive(false);
                break;
            case Direction.Left:
                moveLeftButton.SetActive(false);
                break;
            case Direction.Right:
                moveRightButton.SetActive(false);
                break;
        }
    }

    public void EnableAllMovement()
    {
        moveAheadButton.SetActive(true);
        moveBackButton.SetActive(true);
        moveLeftButton.SetActive(true);
        moveRightButton.SetActive(true);
    }

    private void UpdateInteractionButton()
    {
        emergencyButton.SetActive(canCallEmergency);
        fleeButton.SetActive(canFlee);
        
        interactionBarIsDirty = false;
    }
}

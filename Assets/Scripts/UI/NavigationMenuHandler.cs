using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class NavigationMenuHandler : MonoBehaviour
{
    public GameObject movementBar;
    public GameObject moveNorthButton;
    public GameObject moveSouthButton;
    public GameObject moveWestButton;
    public GameObject moveEastButton;
    public GameObject enterInteractionButton;
    public GameObject exitInteractionButton;

    public GameObject interactionBar;
    public GameObject nameLabel;
    public GameObject titleLabel;
    public GameObject messageBox;
    public GameObject currentAddressLabel;

    public GameObject emergencyButton;
    public GameObject personalAnswerButton;
    public GameObject professionalAnswerButton;
    public GameObject doNotSpeakButton;
    public GameObject fleeButton;

    private GameObject activeBar;

    private GameObject player;

    private bool canCallEmergency = true;
    private bool canFlee = true;
    private bool interactionBarIsDirty = false;

    private List<GameObject> movementButtons;

    private void Awake()
    {
        #region Null Checks
        if (movementBar == null)
        {
            throw new System.Exception("Movement Bar not set");
        }

        if (interactionBar == null)
        {
            throw new System.Exception("Interaction Bar not set");
        }

        if (moveNorthButton == null || moveSouthButton == null || moveWestButton == null || moveEastButton == null)
        {
            throw new System.Exception("One or more movement buttons not set");
        }

        if (enterInteractionButton == null || exitInteractionButton == null)
        {
            throw new System.Exception("Enter or Exit Interaction Button not set");
        }

        if (nameLabel == null)
        {
            throw new System.Exception("Name label not set");
        }

        if (titleLabel == null)
        {
            throw new System.Exception("Title label not set");
        }

        if (messageBox == null)
        {
            throw new System.Exception("Message box not set");
        }

        if (emergencyButton == null || personalAnswerButton == null || professionalAnswerButton == null || doNotSpeakButton == null || fleeButton == null)
        {
            throw new System.Exception("One or more interaction buttons not set");
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null )
        { 
            throw new System.Exception("Player not found");
        }
        #endregion

        movementBar.SetActive(true);
        activeBar = movementBar;

        interactionBar.SetActive(false);

        movementButtons = new List<GameObject>
        {
            moveNorthButton,
            moveSouthButton,
            moveWestButton,
            moveEastButton
        };
    }

    private void Start()
    {
        UpdateNavMenuAfterMovement();
    }

    private void Update()
    {
        if (activeBar == interactionBar && interactionBarIsDirty)
        {
            UpdateInteractionButton();
        }

        ActiveBarSanitizer();
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
            case Direction.North:
                moveNorthButton.SetActive(false);
                break;
            case Direction.South:
                moveSouthButton.SetActive(false);
                break;
            case Direction.West:
                moveWestButton.SetActive(false);
                break;
            case Direction.East:
                moveEastButton.SetActive(false);
                break;
        }
    }

    public void UpdateNavMenuAfterMovement()
    {
        

        foreach (GameObject button in movementButtons)
        {
            button.GetComponent<NavButtonScript>().CheckCanMove();
        }

        string currentAddress = player.GetComponent<MovementScript>().GetCurrentAddress();
        currentAddressLabel.GetComponent<TMPro.TextMeshProUGUI>().text = currentAddress;
    }

    private void UpdateInteractionButton()
    {
        emergencyButton.SetActive(canCallEmergency);
        fleeButton.SetActive(canFlee);
        
        interactionBarIsDirty = false;
    }

    // If by any reason both bars are active, deactivate the one that should not be active
    private void ActiveBarSanitizer()
    {
        if (activeBar == movementBar)
        {
            if (interactionBar.activeSelf)
            {
                interactionBar.SetActive(false);
            }
        }

        if (activeBar == interactionBar)
        {
            if (movementBar.activeSelf)
            {
                movementBar.SetActive(false);
            }
        }
    }
}

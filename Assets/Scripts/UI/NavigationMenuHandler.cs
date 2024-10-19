using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Would be better described as NavigationMenuManager, but changing it now would require 
/// changing the references in the scene, and/or causing unpredicted behavior.
/// 
/// Handles all the UI elements related to navigation and interaction with NPCs.
/// Probably the logic of the interactions would be better handled in a separate script.
/// </summary>
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
    public GameObject deliveryBox;
    public GameObject currentAddressLabel;

    public GameObject emergencyButton;
    public GameObject personalAnswerButton;
    public GameObject professionalAnswerButton;
    public GameObject doNotSpeakButton;
    public GameObject fleeButton;

    public GameObject cameraWorldView;

    private GameObject activeBar;

    private GameObject player;
    private Characters activeNPC;
    private GameObject activeNpcPortrait;
    private Waypoints currentWaypoint;

    private bool canCallEmergency = true;
    private bool canFlee = true;
    private bool interactionBarIsDirty = false;

    private List<GameObject> movementButtons;
    private List<GameObject> interactionButtons;

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

        currentWaypoint = player.GetComponent<MovementScript>().GetCurrentWaypoint();
        #endregion
    }

    private void Start()
    {
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

        interactionButtons = new List<GameObject>
        {
            emergencyButton,
            personalAnswerButton,
            professionalAnswerButton,
            doNotSpeakButton,
            fleeButton
        };

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
        if (activeBar == interactionBar)
        {
            EndInteraction();
        }
        else
        {
            activeBar.SetActive(false);
        }

        movementBar.SetActive(true);
        activeBar = movementBar;
    }

    public void ActivateInteractionBar()
    {
        if (!currentWaypoint.IsNpcAvailable())
        {
            return;
        }

        if (activeBar == interactionBar)
        {
            EndInteraction();
        }

        if (!deliveryBox.activeSelf)
        {
            deliveryBox.SetActive(true);
        }

        GameManager.instance.SpendInteractionTime();

        activeBar.SetActive(false);

        PickRandomCharacter();

        interactionBar.SetActive(true);
        
        //TODO: unhide the interaction buttons when they get implemented
        foreach (GameObject button in interactionButtons)
        {
            button.SetActive(false);
        }

        activeBar = interactionBar;

        //TODO: if canCallEmergency or CanFlee has changed, set interactionBarIsDirty to true
        interactionBarIsDirty = false; //if true, it will enable emergency button and flee button,
                                       //because there is no logic to handle their states as of now

        
    }

    public void PickRandomCharacter()
    {
        int i = Random.Range(0, currentWaypoint.residents.Count);
        Characters randomCharacter = currentWaypoint.residents[i];

        activeNPC = randomCharacter;
        activeNpcPortrait = Instantiate(activeNPC.characterPrefab, cameraWorldView.transform);
        SetActiveCharacterName(randomCharacter.fullName);
        SetActiveCharacterTitle(randomCharacter.title);
        SetMessage("Hello, do you happen to have my precious package?");
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
        currentWaypoint = player.GetComponent<MovementScript>().GetCurrentWaypoint();
        currentAddressLabel.GetComponent<TextMeshProUGUI>().text = currentWaypoint.GetFullAddress();

        foreach (GameObject button in movementButtons)
        {
            button.GetComponent<NavButtonScript>().CheckCanMove();
        }
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
                EndInteraction();

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

    public Characters GetActiveNPC()
    {
        return activeNPC;
    }

    private void EndInteraction()
    {
        interactionBar.SetActive(false);
        Destroy(activeNpcPortrait);
        activeNPC = null;
    }
}

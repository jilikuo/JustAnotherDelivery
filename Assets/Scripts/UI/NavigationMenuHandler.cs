using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all the UI elements related to navigation and interaction with NPCs in the Delivery Scene.
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

    public GameObject cameraWorldView;

    private GameManager gameManager = GameManager.instance;

    private GameObject activeBar;

    private GameObject player;
    private Characters activeNPC;
    private GameObject activeNpcPortrait;
    private Waypoints currentWaypoint;

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

    public void InteractionButtonPress()
    {
        switch (StorylineManager.instance.state)
        {
            case StorylineState.Inactive:
                ActivateMovementBar();
                break;
            case StorylineState.WaitingDelivery:
                ActivateMovementBar();
                break;
            case StorylineState.WaitingInput:
                StorylineManager.instance.AdvanceInteraction();
                break;
        }
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

    public void OnStartInteraction()
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

        gameManager.SpendInteractionTime();

        activeBar.SetActive(false);

        //TODO: VERIFY THE NEXT NPC, IF IT IS ASSOCIATED TO A SPECIFIC STORYLINE WHICH THE PLAYER HAS THE PACKAGE OF,
        //FIRST NON REPEATABLE, THEN REPEATABLE, PLAY THE SPECIFIC STORYLINE.
        //IF THAT'S NOT THE CASE, PLAY A RANDOM REPEATABLE ONE. (current behaviour: only plays random repeatable ones)
        StorylineManager.instance.PlayRandomRepeatableStoryline();

        interactionBar.SetActive(true);

        activeBar = interactionBar;
    }

    public Characters GetRandomNPC()
    {
        int i = Random.Range(0, currentWaypoint.residents.Count);
        return currentWaypoint.residents[i]; 
    }

    public void SetActiveCharacter(Characters NPC)
    {
        activeNPC = NPC;
        UpdateActiveCharacterInfo();
    }

    public void UpdateActiveCharacterInfo()
    {
        Destroy(activeNpcPortrait);
        if (activeNPC == null)
        {
            Debug.Log("NO NPC FOUND");
            return;
        }
        activeNpcPortrait = Instantiate(activeNPC.characterPrefab, cameraWorldView.transform);
        SetActiveCharacterName(activeNPC.fullName);
        SetActiveCharacterTitle(activeNPC.title);
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

    public void UpdateInteractionButton()
    {
        if (StorylineManager.instance.state == StorylineState.WaitingInput)
        {
            exitInteractionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        }
        else
        {
            exitInteractionButton.GetComponentInChildren<TextMeshProUGUI>().text = "Exit";
        }
        
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

    public void EndInteraction()
    {
        if (StorylineManager.instance.state != StorylineState.Inactive)
        {
            StorylineManager.instance.ResetCurrentChapterProgress();
        }

        interactionBar.SetActive(false);
        Destroy(activeNpcPortrait);
        activeNPC = null;
    }

    public bool IsPlayerInteracting()
    {
        return (activeBar == interactionBar);
    }

    public bool IsDeliveryBoxAvailable()
    {
        return deliveryBox.activeSelf;
    }
}

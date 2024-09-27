using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class NavigationMenuHandler : MonoBehaviour
{
    public GameObject debugMenu;
    public GameObject debugMenuButton;

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
        if (movementBar == null || interactionBar == null)
        {
            throw new System.Exception("Either Movement Bar or Interaction bar not set in NavigationMenuHandler");
        }

        if (moveAheadButton == null || moveBackButton == null || moveLeftButton == null || moveRightButton == null)
        {
            throw new System.Exception("One or more movement buttons not set in NavigationMenuHandler");
        }

        if (nameLabel == null || titleLabel == null || messageBox == null)
        {
            throw new System.Exception("Either labels or message box not set in NavigationMenuHandler");
        }

        if (emergencyButton == null || personalAnswerButton == null || professionalAnswerButton == null || doNotSpeakButton == null || fleeButton == null)
        {
            throw new System.Exception("One or more interaction buttons not set in NavigationMenuHandler");
        }
        #endregion

#if (UNITY_EDITOR)
        debugMenuButton.SetActive(true);
#endif

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
        if (canCallEmergency)
        {
            emergencyButton.SetActive(true);
        }
        else
        {
            emergencyButton.SetActive(false);
        }
        if (canFlee)
        {
            fleeButton.SetActive(true);
        }
        else
        {
            fleeButton.SetActive(false);
        }

        interactionBarIsDirty = false;
    }

    public void DebugNewName(TMP_InputField name)
    {
        SetActiveCharacterName(name.text);
        Debug.Log(name.text);
    }

    public void DebugNewTitle(TMP_InputField title)
    {
        SetActiveCharacterTitle(title.text);
        Debug.Log(title.text);
    }

    public void DebugNewMessage(TMP_InputField message)
    {
        SetMessage(message.text);
        Debug.Log(message.text);
    }

    public void SwitchDebugMenu()
    {
        Debug.Log("Switching Debug Menu");
            debugMenu.SetActive(true);
     
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradePanelManagerTextBase : UpgradePanelManagerBase
{
    protected override void UpdateDisplay()
    {
        currentStateText.text = GetCurrentValue();
        nextStateText.text = "Upgrade to " + GetNextValue();
    }

    protected abstract string GetCurrentValue();
    protected abstract string GetNextValue();
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradePanelManagerTextBase : UpgradePanelManagerBase
{
    protected override void SetVars()
    {
        base.SetVars();
        currentStateBottomText.text = "";
        nextStateBottomText.text = "";
    }

    protected override void UpdateDisplay()
    {
        currentStateCenterText.text = GetCurrentValue();
        nextStateCenterText.text = GetNextValue();
    }

    protected abstract string GetCurrentValue();
    protected abstract string GetNextValue();
}

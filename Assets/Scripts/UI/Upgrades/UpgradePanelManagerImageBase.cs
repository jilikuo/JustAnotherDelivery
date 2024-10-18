using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradePanelManagerImageBase : UpgradePanelManagerBase
{
    GameObject currentImage;
    GameObject nextImage;
    protected override void SetVars()
    {
        base.SetVars();
        currentStateCenterText.text = "";
        currentStateCenterText.enabled = false;
        nextStateCenterText.text = "";
        nextStateCenterText.enabled = false;
    }

    protected override void UpdateDisplay()
    {
        if (currentImage != null)
        {
            Destroy(currentImage);
        }
        currentImage = GetCurrentImage();
        if (currentImage != null)
        {
            currentImage.transform.SetParent(currentStateBackground.transform, false);
            currentImage.CenterAndStretchToParent();
        }
        string currentText = GetCurrentText();
        if (currentText != null) {
            currentStateBottomText.text = currentText;
            currentStateBottomText.enabled = true;
        }
        else
        {
            currentStateBottomText.text = "";
            currentStateBottomText.enabled = false;
        }

        if (nextImage != null)
        {
            Destroy(nextImage);
        }
        nextImage = GetNextImage();
        if (nextImage != null)
        {
            nextImage.transform.SetParent(nextStateBackground.transform, false);
            // TODO: Fix scaling
            nextImage.CenterAndStretchToParent();
        }
        string nextText = GetNextText();
        if (nextText != null) {
            nextStateBottomText.text = nextText;
            nextStateBottomText.enabled = true;
        }
        else
        {
            nextStateBottomText.text = "";
            nextStateBottomText.enabled = false;
        }
    }

    protected abstract GameObject GetCurrentImage();
    protected abstract GameObject GetNextImage();
    protected abstract string GetCurrentText();
    protected abstract string GetNextText();
}

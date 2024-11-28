using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleImageOnButtonClickHandler : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject defaultImage;
    [SerializeField] private GameObject altImage;
    [SerializeField] private float toggleTime = 0.5f;

    private void Start()
    {
        if (defaultImage == null)
        {
            Debug.LogError("defaultImage not set");

        }
        if (altImage == null)
        {
            Debug.LogError("altImage not set");

        }
        SetActiveImage(true);

        if (button == null)
        {
            button = gameObject.GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError("button not set");

            }
        }
        button.onClick.AddListener(ActivateAltImage);
    }

    private void SetActiveImage(bool setDefaultActive)
    {
        defaultImage.SetActive(setDefaultActive);
        altImage.SetActive(!setDefaultActive);
        button.enabled = setDefaultActive;
    }

    public void ActivateAltImage()
    {
        if (altImage.activeSelf)
        {
            return;
        }
        SetActiveImage(false);
        StartCoroutine(OnFinishToggle());
    }

    private IEnumerator OnFinishToggle()
    {
        var time = toggleTime;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }

        SetActiveImage(true);
    }
}

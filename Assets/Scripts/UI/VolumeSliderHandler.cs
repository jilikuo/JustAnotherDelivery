using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.ContentSizeFitter;

public class VolumeSliderHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider slider;

    private void Start()
    {
        var volumeLevel = GameManager.instance.GetVolumeLevel();
        slider.value = volumeLevel;
        OnChangeSlider(volumeLevel);
    }

    public void OnChangeSlider(float value)
    {
        GameManager.instance.SetVolumeLevel(value);
        audioSource.Play();
    }
}

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
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string volumeLevelKey = "volumeLevel";

    private void Start()
    {
        var volumeLevel = PlayerPrefs.GetFloat(volumeLevelKey, 1);
        slider.value = volumeLevel;
        OnChangeSlider(volumeLevel);
    }

    public void OnChangeSlider(float value)
    {
        float volumeLevel = Mathf.Log10(value) * 20;

        mixer.SetFloat("Volume", volumeLevel);

        PlayerPrefs.SetFloat(volumeLevelKey, value); // Save the value, instead of the volume, to avoid back-conversion
        PlayerPrefs.Save();

        audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClickSoundHandler : MonoBehaviour
{
    public string buttonClickAudioSourceTag = "ButtonClickAudioSource";
    public AudioClip audioToPlay;

    private void Start()
    {
        var buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    private void PlaySound()
    {
        var audioSource = GameObject.FindGameObjectWithTag(buttonClickAudioSourceTag).GetComponent<AudioSource>();

        //Prevent no sound from playing
        if (audioToPlay != null)
        {
            audioSource.clip = audioToPlay;
        }

        audioSource.Play();
    }
}

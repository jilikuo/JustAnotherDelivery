using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClickSoundHandler : CloseOnEscHandler
{
    public AudioClip audioClip;

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
        var generalAudioSource = GameObject.FindGameObjectWithTag("GeneralAudioSource").GetComponent<AudioSource>();
        generalAudioSource.PlayOneShot(audioClip);
    }
}

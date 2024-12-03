using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource ambientalSounds;
    public List<AudioClip> ambientSounds;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (backgroundMusic != null)
        {
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.loop = true;
                backgroundMusic.Play();
            }
        }

        if (!ambientalSounds.isPlaying)
        {

            ambientalSounds.clip = ambientSounds[Random.Range(0, ambientSounds.Count)];
            ambientalSounds.Play();
        }

        this.transform.position = player.transform.position;
    }
}

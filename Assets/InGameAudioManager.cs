using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameAudioManager : MonoBehaviour
{
    [SerializeField] AudioClip newSong;
    private GameObject audioManager;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager");
        if (audioManager) {
            AudioSource source = audioManager.GetComponent<AudioSource>();
            source.clip = newSong;
            source.Play();
        }
        
    }

}

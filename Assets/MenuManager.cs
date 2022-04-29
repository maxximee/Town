using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource music;

    private void Awake() {
        DontDestroyOnLoad(music);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailToVolume : MonoBehaviour
{

[SerializeField] private ParticleSystem _tailEmissionModule;
[SerializeField] private AudioSource _audioSource;


    // Update is called once per frame
    void Update()
    {
        _audioSource.volume = _tailEmissionModule.main.startSize.constantMax * 3;
    }
}

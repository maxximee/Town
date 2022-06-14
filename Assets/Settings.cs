using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    [Header("Controls Settings")]
    [SerializeField] private GameObject tiltOnToggle;
    [SerializeField] private GameObject tiltOffToggle;

    private void Start()
    {
        Boolean isGyro = Manager.isGyroControls();

        if (isGyro)
        {
            tiltOffToggle.SetActive(false);
            tiltOnToggle.SetActive(true);
        }
        else
        {
            tiltOffToggle.SetActive(true);
            tiltOnToggle.SetActive(false);
        }

    }

    public void switchGyroControls()
    {
        bool newState = Manager.ToggleGyroControls();
        if (newState)
        {
            tiltOnToggle.SetActive(true);
            tiltOffToggle.SetActive(false);
        } else {
            tiltOnToggle.SetActive(false);
            tiltOffToggle.SetActive(true);
        }
    }

}

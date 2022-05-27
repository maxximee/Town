using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoWalletManager : MonoBehaviour
{
    [SerializeField] private GameObject NoWalletPanel;
    void Awake() {
        if (PlayerPrefs.HasKey("playerAddress")) {
            Manager.PlayerAddress = PlayerPrefs.GetString("playerAddress");
        }

        if (String.IsNullOrEmpty(Manager.PlayerAddress)) {
            NoWalletPanel.SetActive(true);
        }   else {
            NoWalletPanel.SetActive(false); 
        }
    }
}

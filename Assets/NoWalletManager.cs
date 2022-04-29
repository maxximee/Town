using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoWalletManager : MonoBehaviour
{
    [SerializeField] private GameObject NoWalletPanel;
    void Awake() {
        // TODO should be replaced with pincode UI
        if (PlayerPrefs.HasKey("playerPk")) {
            Manager.PlayerPK = PlayerPrefs.GetString("playerPk");
            Manager.PlayerAddress = PlayerPrefs.GetString("playerAddress");
        }

        if (String.IsNullOrEmpty(Manager.PlayerPK)) {
            NoWalletPanel.SetActive(true);
        }   else {
            NoWalletPanel.SetActive(false); 
        }
    }
}

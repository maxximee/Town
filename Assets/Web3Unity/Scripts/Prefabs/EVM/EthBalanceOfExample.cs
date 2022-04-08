using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EthBalanceOfExample : MonoBehaviour
{
    public TextMeshProUGUI balanceText;

    async void Start()
    {
        string chain = "polygon";
        string network = "mumbai";
        string rpc = "https://rpc-mumbai.matic.today";
        string account = "0x3d820337ed4041D4469A830B21A15FEB9C1ac9dC";

        string balance = await EVM.BalanceOf(chain, network, account, rpc);
        print(balance);


        float wei = float.Parse(balance.ToString());
        float decimals = 1000000000000000000; // 18 decimals
        float atoms = wei / decimals;
        balanceText.text = (Mathf.Round(atoms * 1000f) / 1000f).ToString();
    }
}

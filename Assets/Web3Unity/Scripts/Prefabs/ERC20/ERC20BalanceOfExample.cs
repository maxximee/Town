using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ERC20BalanceOfExample : MonoBehaviour
{
    public TextMeshProUGUI balanceText;

    async void Start()
    {
        string chain = "polygon";
        string network = "mumbai";
        string contract = "0x0400c0624a90CA9097F8F248F4c04173b8C3f8ea";
        string account = "0x3d820337ed4041D4469A830B21A15FEB9C1ac9dC";
        string rpc = "https://rpc-mumbai.matic.today";

        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, account, rpc);
        print(balanceOf);

        float wei = float.Parse(balanceOf.ToString());
        float decimals = 1000000000000000000; // 18 decimals
        float atoms = wei / decimals;
        balanceText.text = System.Convert.ToDecimal(atoms).ToString();
    }
}

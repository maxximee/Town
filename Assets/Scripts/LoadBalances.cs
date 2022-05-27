using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ToastForUnity.Resources.ToastSettings.Stylish;
using ToastForUnity.Script.Core;
using ToastForUnity.Script.Enum;
using Nethereum.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using System.Numerics;
using TMPro;

public class LoadBalances : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI atomBalanceText;
    [SerializeField] private TextMeshProUGUI maticBalanceText;

    [SerializeField] private TextMeshProUGUI atomWalletBalanceText;
    [SerializeField] private TextMeshProUGUI maticWalletBalanceText;

    [SerializeField] private GameObject storeButton;
    [SerializeField] private Rewards rewardManager;

    public ParentController ParentController;

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; }
    }



    // Start is called before the first frame update
    async void Start()
    {
        if (!String.IsNullOrEmpty(Manager.PlayerAddress))
        {
            loadBalances();
        }
    }

    async void loadBalances()
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var web3 = new Web3(url);

        var balanceOfFunctionMessage = new BalanceOfFunction()
        {
            Owner = Manager.PlayerAddress,
        };

        var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
        var balance = await balanceHandler.QueryAsync<BigInteger>(Manager.TokenContractAddress, balanceOfFunctionMessage);

        float wei = float.Parse(balance.ToString());
        float decimals = (float)Manager.TokenDecimal; // 18 decimals
        float atoms = wei / decimals;
        float localAtoms = 0f;
        if (PlayerPrefs.HasKey(Manager.AtomsPrefs))
        {
            localAtoms = PlayerPrefs.GetFloat(Manager.AtomsPrefs);
        }
        if (localAtoms > 0)
        {
            atomBalanceText.color = Color.red;
            atomWalletBalanceText.color = Color.red;
            atomWalletBalanceText.text = Convert.ToDecimal(atoms).ToString() + "(+" + Convert.ToDecimal(localAtoms).ToString() + ")";
            storeButton.SetActive(true);
            double result = (double)Manager.TokenDecimal * localAtoms;
            storeButton.GetComponent<Button>().onClick.AddListener(async () =>
                {
                    var status = await rewardManager.sendRewardsAsync(new BigInteger(result).ToString());
                    PlayerPrefs.SetFloat(Manager.AtomsPrefs, 0f);
                    // TODO 1) add fee for transfering
                    if (status == 1)
                    {
                        loadBalances();
                    }
                    else
                    {
                        StylishPop("StylishToast-Warning", new StylistToastModel()
                        {
                            Title = "Warning",
                            Content = "Loading balances failed, check you connection..."
                        });
                        Debug.LogWarning("transaction failed, try again...");
                    }
                }
                );
        }
        else
        {
            atomBalanceText.color = Color.white;
            atomWalletBalanceText.color = Color.white;
            atomWalletBalanceText.text = Convert.ToDecimal(atoms + localAtoms).ToString();
            storeButton.SetActive(false);
        }
        atomBalanceText.text = Convert.ToDecimal(atoms + localAtoms).ToString();

        var maticBalance = await web3.Eth.GetBalance.SendRequestAsync(Manager.PlayerAddress);

        float maticWei = float.Parse(maticBalance.ToString());
        float matic = maticWei / decimals;
        maticBalanceText.text = (Mathf.Round(matic * 1000f) / 1000f).ToString();
        maticWalletBalanceText.text = (Mathf.Round(matic * 1000f) / 1000f).ToString();
    }

    private void StylishPop(string stylishName, StylistToastModel toastModel)
    {
        Toast.PopOut<StylistToastView>(stylishName, toastModel,
            ParentController.GetParent(ToastPosition.BottomCenter));
    }
}

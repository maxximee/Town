using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunction : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public string Owner { get; set; }
    }



    // Start is called before the first frame update
    async void Start()
    {
        if (!String.IsNullOrEmpty(Manager.PlayerPK)) {
            loadBalances();
        } 
    }

    async void loadBalances() {
        var url = Manager.infuraMumbaiEndpointUrl;
            var privateKey = Manager.PlayerPK;
            var account = new Account(privateKey);
            var web3 = new Web3(account, url);

            var balanceOfFunctionMessage = new BalanceOfFunction()
            {
                Owner = account.Address,
            };

            var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            var balance = await balanceHandler.QueryAsync<BigInteger>(Manager.TokenContractAddress, balanceOfFunctionMessage);

            float wei = float.Parse(balance.ToString());
            float decimals = Manager.TokenDecimal; // 18 decimals
            float atoms = wei / decimals;
            atomBalanceText.text = Convert.ToDecimal(atoms).ToString();

            var maticBalance = await web3.Eth.GetBalance.SendRequestAsync(Manager.PlayerAddress);

            float maticWei = float.Parse(maticBalance.ToString());
            float matic = maticWei / decimals;
            maticBalanceText.text = (Mathf.Round(matic * 1000f) / 1000f).ToString();
    }


}

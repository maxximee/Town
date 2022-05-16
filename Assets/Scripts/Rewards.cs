using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.Extensions;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;
using Nethereum.StandardTokenEIP20.ContractDefinition;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;
using UnityEngine.UI;


public class Rewards : MonoBehaviour
{


    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }



    private Web3 web3;

    private void Start()
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.BankPK;
        var account = new Account(privateKey, Manager.ChainId);
        web3 = new Web3(account, url);
    }

    public async Task<BigInteger> sendRewardsAsync(string value)
    {
        var transferFunction = new TransferFunction();
        transferFunction.To = Manager.PlayerAddress;
        transferFunction.Amount = BigInteger.Parse(value);

        var contractHandler = web3.Eth.GetContractHandler(Manager.TokenContractAddress);
        Debug.Log("sending atoms...");
        var transferFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(transferFunction);
        Debug.Log("transfered " + value + " to player. Transaction hash: " + transferFunctionTxnReceipt.TransactionHash);
        Manager.showToast("transfered " + value + " to player. Transaction hash: " + transferFunctionTxnReceipt.TransactionHash, 2);
        return transferFunctionTxnReceipt.Status.Value;
    }
}

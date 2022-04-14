using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
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
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using UnityEngine;
using UnityEngine.UI;


public class Rewards : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(TransferEther());
        StartCoroutine(TransferAtom());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "_to", 1)]
        public string To { get; set; }
        [Parameter("uint256", "_amount", 2)]
        public BigInteger Amount { get; set; }
    }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "_from", 1, true)]
        public virtual string From { get; set; }

        [Parameter("address", "_to", 2, true)]
        public virtual string To { get; set; }

        [Parameter("uint256", "_amount", 3, false)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase
    {
        public static EventABI GetEventABI()
        {
            return EventExtensions.GetEventABI<TransferEventDTO>();
        }
    }



    public partial class TransferFunction : TransferFunctionBase
    {
    }


    public IEnumerator TransferAtom()
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.BankPK;
        var account = Manager.BankAddress;
      

        var transactionTransferRequest = new TransactionSignedUnityRequest(url, privateKey);

        var newAddress = Manager.PlayerAddress;

        var transactionMessage = new TransferFunction
        {
            To = newAddress,
            Amount = 10,

        };

        yield return transactionTransferRequest.SignAndSendTransaction(transactionMessage, Manager.TokenContractAddress);

        var transactionTransferHash = transactionTransferRequest.Result;

        Debug.Log("Transfer txn hash:" + transactionTransferHash);

        //var transactionReceiptPolling = new TransactionReceiptPollingRequest(url);
        //yield return transactionReceiptPolling.PollForReceipt(transactionTransferHash, 2);
        //var transferReceipt = transactionReceiptPolling.Result;

        //var transferEvent = transferReceipt.DecodeAllEvents<TransferEventDTO>();
        //Debug.Log("Transferd amount from event: " + transferEvent[0].Event.Value);

        //var getLogsRequest = new EthGetLogsUnityRequest(url);

        //var eventTransfer = TransferEventDTO.GetEventABI();
        //yield return getLogsRequest.SendRequest(eventTransfer.CreateFilterInput(Manager.TokenContractAddress, account));

        //var eventDecoded = getLogsRequest.Result.DecodeAllEvents<TransferEventDTO>();

        //Debug.Log("Transferd amount from get logs event: " + eventDecoded[0].Event.Value);
    }


    public IEnumerator TransferMatic()
    {
        decimal GasPriceGwei = 2;
        var ethTransfer = new EthTransferUnityRequest(Manager.infuraMumbaiEndpointUrl, Manager.BankPK, Manager.ChainId);
        Debug.Log("Legacy mode");
        //I am forcing the legacy mode but also I am including the gas price
        ethTransfer.UseLegacyAsDefault = true;

        yield return ethTransfer.TransferEther(Manager.PlayerAddress, 1m, GasPriceGwei);

        if (ethTransfer.Exception != null)
        {
            Debug.Log(ethTransfer.Exception.Message);
            yield break;
        }

        string TransactionHash = ethTransfer.Result;
        Debug.Log("Transfer transaction hash:" + TransactionHash);

        //create a poll to get the receipt when mined
        var transactionReceiptPolling = new TransactionReceiptPollingRequest(Manager.infuraMumbaiEndpointUrl);
        //checking every 2 seconds for the receipt
        yield return transactionReceiptPolling.PollForReceipt(TransactionHash, 2);

        Debug.Log("Transaction mined");

        var balanceRequest = new EthGetBalanceUnityRequest(Manager.infuraMumbaiEndpointUrl);
        yield return balanceRequest.SendRequest(Manager.PlayerAddress, BlockParameter.CreateLatest());

        string BalanceAddressTo = UnitConversion.Convert.FromWei(balanceRequest.Result.Value).ToString();

        Debug.Log("Balance of account:" + BalanceAddressTo);
    }
 
}

using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Nethereum.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using System.Numerics;
using System;

public class DragonLevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI accelPoints;
    [SerializeField] private TextMeshProUGUI topSpeedPoints;
    [SerializeField] private TextMeshProUGUI dietPoints;
    [SerializeField] private TextMeshProUGUI yieldPoints;

    [SerializeField] private TextMeshProUGUI availablePoints;

    [SerializeField] private TextMeshProUGUI dragonIdText;

    [SerializeField] private GameObject loginPanel;

    private int _accel = 0;
    private int _topSpeed = 0;
    private int _diet = 0;
    private int _yield = 0;

    private int _availablePoints = 0;

    // TODO depends on rarity    
    private int PointsPerLevel = 4;

    // TODO depends on rarity + level
    private BigInteger TokensCostToLevelUp = 15;

    public partial class LevelUpFunction : LevelUpFunctionBase { }

    [Function("levelUp")]
    public class LevelUpFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_id", 1)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "_cost", 2)]
        public virtual BigInteger Cost { get; set; }
        [Parameter("uint256", "_accel", 3)]
        public virtual BigInteger Accel { get; set; }
        [Parameter("uint256", "_topSpeed", 4)]
        public virtual BigInteger TopSpeed { get; set; }
        [Parameter("uint256", "_yield", 5)]
        public virtual BigInteger Yield { get; set; }
        [Parameter("uint256", "_diet", 6)]
        public virtual BigInteger Diet { get; set; }
        [Parameter("address", "_dragonOwner", 7)]
        public virtual string DragonOwner { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve", "bool")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    private void Awake()
    {
        TokensCostToLevelUp = TokensCostToLevelUp * Manager.TokenDecimal;
    }

    public async void ApproveLevelUp()
    {
        if (!String.IsNullOrEmpty(Manager.PlayerPK))
        {

            _availablePoints = PointsPerLevel;
            availablePoints.text = _availablePoints.ToString();

            var url = Manager.infuraMumbaiEndpointUrl;
            var account = new Account(Manager.PlayerPK, Manager.ChainId);
            var web3 = new Web3(account, url);
            var contractHandler = web3.Eth.GetContractHandler(Manager.TokenContractAddress);

            var approveFunction = new ApproveFunction();
            approveFunction.Spender = Manager.NftContractAddress;
            approveFunction.Amount = TokensCostToLevelUp;
            var approveFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(approveFunction);
            Debug.Log("approve transaction receipt: " + approveFunctionTxnReceipt.TransactionHash);
            Manager.showToast("approve transaction receipt: " + approveFunctionTxnReceipt.TransactionHash, 2);
        }
        else
        {
            loginPanel.SetActive(true);
        }

    }

    public async void SaveChanges()
    {
        int dragonId = int.Parse(dragonIdText.text);

        var url = Manager.infuraMumbaiEndpointUrl;
        var account = new Account(Manager.BankPK, Manager.ChainId);
        var web3 = new Web3(account, url);
        var contractHandler = web3.Eth.GetContractHandler(Manager.NftContractAddress);

        var levelUpFunction = new LevelUpFunction();
        levelUpFunction.Id = dragonId;
        levelUpFunction.Cost = TokensCostToLevelUp;
        levelUpFunction.Accel = _accel * 10;
        levelUpFunction.TopSpeed = _topSpeed * 10;
        levelUpFunction.Yield = _yield * 10;
        levelUpFunction.Diet = _diet * 10;
        levelUpFunction.DragonOwner = Manager.PlayerAddress;
        var levelUpFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(levelUpFunction);
        Debug.Log("level up transaction receipt: " + levelUpFunctionTxnReceipt.TransactionHash);
        Manager.showToast("level up transaction receipt: " + levelUpFunctionTxnReceipt.TransactionHash, 2);
    }

    public void IncreaseAcceleration()
    {
        if (_availablePoints > 0)
        {
            _accel++;
            accelPoints.text = _accel.ToString();
            _availablePoints--;
            availablePoints.text = _availablePoints.ToString();
        }
    }

    public void IncreaseTopSpeed()
    {
        if (_availablePoints > 0)
        {
            _topSpeed++;
            topSpeedPoints.text = _topSpeed.ToString();
            _availablePoints--;
            availablePoints.text = _availablePoints.ToString();
        }
    }

    public void IncreaseDiet()
    {
        if (_availablePoints > 0)
        {
            _diet++;
            dietPoints.text = _diet.ToString();
            _availablePoints--;
            availablePoints.text = _availablePoints.ToString();
        }
    }

    public void IncreaseYield()
    {
        if (_availablePoints > 0)
        {
            _yield++;
            yieldPoints.text = _yield.ToString();
            _availablePoints--;
            availablePoints.text = _availablePoints.ToString();
        }
    }

    public void DecreaseAcceleration()
    {
        if (_accel > 0)
        {
            _accel--;
            accelPoints.text = _accel.ToString();
            Mathf.Min(PointsPerLevel, _availablePoints++);
            availablePoints.text = _availablePoints.ToString();
        }
    }

    public void DecreaseTopSpeed()
    {
        if (_topSpeed > 0)
        {
            _topSpeed--;
            topSpeedPoints.text = _topSpeed.ToString();
            Mathf.Min(PointsPerLevel, _availablePoints++);
            availablePoints.text = _availablePoints.ToString();
        }
    }

    public void DecreaseDiet()
    {
        if (_diet > 0)
        {
            _diet--;
            dietPoints.text = _diet.ToString();
            Mathf.Min(PointsPerLevel, _availablePoints++);
            availablePoints.text = _availablePoints.ToString();
        }
    }

    public void DecreaseYield()
    {
        if (_yield > 0)
        {
            _yield--;
            yieldPoints.text = _yield.ToString();
            Mathf.Min(PointsPerLevel, _availablePoints++);
            availablePoints.text = _availablePoints.ToString();
        }
    }
}


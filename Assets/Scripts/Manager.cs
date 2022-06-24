using System;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using static LoadMarketItems;

public class Manager : Singleton<Manager>
{

    #region project specific
    public static readonly string infuraMumbaiEndpointUrl = "https://polygon-mumbai.infura.io/v3/abc77529db404a6797d4cb290899eac2";
    public static readonly string infuraProjectId = "27MeYJNva2h8xpyWjOIiTikzt9y";
    public static readonly string infuraProjectSecret = "f46e701998a36bf5989f7906b3d5b066";
    public static readonly string PolygonRpcUrl = "https://rpc-mumbai.matic.today";
    public static readonly string Chain = "polygon";
    public static readonly string Network = "mumbai";
    public static readonly BigInteger ChainId = 80001;
    public static readonly string MarketplaceContractAddress = "0xb8F9157453F6eeDD62E5c5A23a6845E320A8C128";
    public static readonly string NftContractAddress = "0x9523214EC3658931ecEA366033E22e9F2eC4c148";
    public static readonly string TokenContractAddress = "0x0400c0624a90CA9097F8F248F4c04173b8C3f8ea";
    public static readonly BigInteger TokenDecimal = 1000000000000000000;
    public static readonly BigInteger ListingFee = 25000000000000000;
    public static readonly string AtomsPrefs = "localAtoms";
    #endregion

    #region contract abis
    public static readonly string atomAbi = "[{\"inputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"constructor\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"owner\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"value\",\"type\": \"uint256\"}],\"name\": \"Approval\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"previousOwner\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"newOwner\",\"type\": \"address\"}],\"name\": \"OwnershipTransferred\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"from\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"value\",\"type\": \"uint256\"}],\"name\": \"Transfer\",\"type\": \"event\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"owner\",\"type\": \"address\"},{\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"}],\"name\": \"allowance\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"approve\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"account\",\"type\": \"address\"}],\"name\": \"balanceOf\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"burn\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"account\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"burnFrom\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"decimals\",\"outputs\": [{\"internalType\": \"uint8\",\"name\": \"\",\"type\": \"uint8\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"subtractedValue\",\"type\": \"uint256\"}],\"name\": \"decreaseAllowance\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"addedValue\",\"type\": \"uint256\"}],\"name\": \"increaseAllowance\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"mint\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"name\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"owner\",\"outputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"renounceOwnership\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"symbol\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"totalSupply\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"transfer\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"from\",\"type\": \"address\"},{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"transferFrom\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"newOwner\",\"type\": \"address\"}],\"name\": \"transferOwnership\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"}]";
    public static readonly string marketAbi = "[{\"inputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"constructor\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"indexed\": false,\"internalType\": \"address\",\"name\": \"seller\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"address\",\"name\": \"buyer\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"indexed\": false,\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"name\": \"MarketItemCreated\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"indexed\": false,\"internalType\": \"address\",\"name\": \"seller\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"address\",\"name\": \"buyer\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"indexed\": false,\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"name\": \"MarketItemSold\",\"type\": \"event\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"}],\"name\": \"createMarketItem\",\"outputs\": [],\"stateMutability\": \"payable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"}],\"name\": \"createMarketSale\",\"outputs\": [],\"stateMutability\": \"payable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"itemId\",\"type\": \"uint256\"}],\"name\": \"deleteMarketItem\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"fetchActiveItems\",\"outputs\": [{\"components\": [{\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"address payable\",\"name\": \"seller\",\"type\": \"address\"},{\"internalType\": \"address payable\",\"name\": \"buyer\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"internalType\": \"struct NFTMarketplace.MarketItem[]\",\"name\": \"\",\"type\": \"tuple[]\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"fetchMyCreatedItems\",\"outputs\": [{\"components\": [{\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"address payable\",\"name\": \"seller\",\"type\": \"address\"},{\"internalType\": \"address payable\",\"name\": \"buyer\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"internalType\": \"struct NFTMarketplace.MarketItem[]\",\"name\": \"\",\"type\": \"tuple[]\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"fetchMyPurchasedItems\",\"outputs\": [{\"components\": [{\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"address payable\",\"name\": \"seller\",\"type\": \"address\"},{\"internalType\": \"address payable\",\"name\": \"buyer\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"internalType\": \"struct NFTMarketplace.MarketItem[]\",\"name\": \"\",\"type\": \"tuple[]\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"getListingFee\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"listingFee\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"marketowner\",\"outputs\": [{\"internalType\": \"address payable\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"}]";
    public static readonly string nftAbi = "[{\"inputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"constructor\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"owner\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"approved\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"Approval\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"owner\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"operator\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"bool\",\"name\": \"approved\",\"type\": \"bool\"}],\"name\": \"ApprovalForAll\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"previousOwner\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"newOwner\",\"type\": \"address\"}],\"name\": \"OwnershipTransferred\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"from\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"Transfer\",\"type\": \"event\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"approve\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"owner\",\"type\": \"address\"}],\"name\": \"balanceOf\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"burn\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"getApproved\",\"outputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"owner\",\"type\": \"address\"},{\"internalType\": \"address\",\"name\": \"operator\",\"type\": \"address\"}],\"name\": \"isApprovedForAll\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"name\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"owner\",\"outputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"ownerOf\",\"outputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"renounceOwnership\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"string\",\"name\": \"uri\",\"type\": \"string\"}],\"name\": \"safeMint\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"from\",\"type\": \"address\"},{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"safeTransferFrom\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"from\",\"type\": \"address\"},{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"bytes\",\"name\": \"_data\",\"type\": \"bytes\"}],\"name\": \"safeTransferFrom\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"operator\",\"type\": \"address\"},{\"internalType\": \"bool\",\"name\": \"approved\",\"type\": \"bool\"}],\"name\": \"setApprovalForAll\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"bytes4\",\"name\": \"interfaceId\",\"type\": \"bytes4\"}],\"name\": \"supportsInterface\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"symbol\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"tokenURI\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"from\",\"type\": \"address\"},{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"}],\"name\": \"transferFrom\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"newOwner\",\"type\": \"address\"}],\"name\": \"transferOwnership\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"}]";
    #endregion

    #region crypto wallets
    public static readonly string BankPK = "0xd23da211fdc31983b9d3e82b1dfc4999ccda2e544a8aa6bb89c87c720d754f58";
    public static string PlayerAddress;
    public static string PlayerPK;

    public static string PlayerSafeAddress;
    public static string PlayerSafePK;
    #endregion

    #region game settings
    public static float defaultReward = 10f;
    private static string selectedDragon = "0";

    private static float playerReward = 0;
    private static List<MarketItem> marketItems = new List<MarketItem>();
    #endregion

    #region user settings

    public static BigInteger playerAmountOfDragons = 0;
    private static Dictionary<string, DragonDataModel> dragons = new Dictionary<string, DragonDataModel>();
    private static Boolean gyroControls = false;
    public readonly static string GYRO_PLAYER_PREF = "gyroControls";

    #endregion

    public static void addDragon(DragonDataModel dragon, string dragonNumber)
    {
        Debug.Log("add dragon " + dragonNumber);
        dragons[dragonNumber] = dragon;
    }

    public static DragonDataModel GetDragonDataModel(string dragonNumber)
    {
        Debug.Log("get dragon datamodel " + dragonNumber);
        return dragons[dragonNumber];
    }

    public static DragonDataModel GetSelectedDragonDataModel()
    {
        Debug.Log("get dragon " + selectedDragon);
        return dragons[selectedDragon];
    }


    public static void setSelectedDragon(string dragonNumber)
    {
        Debug.Log("set selected dragon " + dragonNumber);
        selectedDragon = dragonNumber;
    }

    public static string getSelectedDragon()
    {
        return selectedDragon;
    }

    public static void SetMarketItems(List<MarketItem> items)
    {
        marketItems = items;
    }

    public static List<MarketItem> GetMarketItems()
    {
        return marketItems;
    }

    public static void SetGyroControls(Boolean isGyro)
    {
        gyroControls = isGyro;
        PlayerPrefs.SetInt(GYRO_PLAYER_PREF, isGyro ? 1 : 0);
    }

    public static Boolean isGyroControls()
    {
        if (PlayerPrefs.HasKey(GYRO_PLAYER_PREF))
        {
            return (PlayerPrefs.GetInt(GYRO_PLAYER_PREF) == 1 ? true : false);
        }
        return gyroControls;
    }

    public static bool ToggleGyroControls()
    {
        if (isGyroControls())
        {
            SetGyroControls(false);
            return false;
        }
        else
        {
            SetGyroControls(true);
            return true;
        }
    }

    public static float getPlayerReward()
    {
        return playerReward;
    }

    public static void setPlayerReward(float reward)
    {
        playerReward = reward;
    }

}


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
        public static  string PlayerAddress;
        public static  string PlayerPK;

        public static  string PlayerSafeAddress;
        public static  string PlayerSafePK;

    #endregion


    private static string selectedDragon = "0";
    private static List<MarketItem> marketItems = new List<MarketItem>();
    
    public static BigInteger playerAmountOfDragons = 0;


    public static void setSelectedDragon(string dragonNumber)
    {
        selectedDragon = dragonNumber ;
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

    public Text txt;

    public static void showToast(string text,
        int duration)
    {
        _instance.StartCoroutine(showToastCOR(text, duration));
    }

    private static IEnumerator showToastCOR(string text,
        int duration)
    {
        Color orginalColor = _instance.txt.color;

        _instance.txt.text = text;
        _instance.txt.enabled = true;

        //Fade in
        yield return fadeInAndOut(_instance.txt, true, 0.5f);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOut(_instance.txt, false, 0.5f);

        _instance.txt.enabled = false;
        _instance.txt.color = orginalColor;
    }

    static IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.white;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
}


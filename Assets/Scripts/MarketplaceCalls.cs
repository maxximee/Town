using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MarketplaceCalls : MonoBehaviour
{
    private string marketplaceContractAddress = Manager.MarketplaceContractAddress;
    private string mumbaiRpc = Manager.PolygonRpcUrl;
    private string chain = Manager.Chain;
    private string network = Manager.Network;
    private string nftContractAddress = Manager.NftContractAddress;
    // smart contract method to calls
    private string sellItemMethod = "createMarketSale"; // nftContract: address, tokenId: uint256, price: uint256
    private string createSaleItemMethod = "createMarketItem"; // nftContract: address , id: uint256
    private string deleteSaleItemMethod = "deleteMarketItem"; // itemId: uint256
    private string approveMethod = "approve"; // to: address, tokenId: uint256
    private string fetchListedItemsMethod = "fetchMyCreatedItems"; // -
    // abi in json format
    private string abi = "[{\"inputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"constructor\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"indexed\": false,\"internalType\": \"address\",\"name\": \"seller\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"address\",\"name\": \"buyer\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"indexed\": false,\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"name\": \"MarketItemCreated\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"indexed\": false,\"internalType\": \"address\",\"name\": \"seller\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"address\",\"name\": \"buyer\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"indexed\": false,\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"name\": \"MarketItemSold\",\"type\": \"event\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"}],\"name\": \"createMarketItem\",\"outputs\": [],\"stateMutability\": \"payable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"}],\"name\": \"createMarketSale\",\"outputs\": [],\"stateMutability\": \"payable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"itemId\",\"type\": \"uint256\"}],\"name\": \"deleteMarketItem\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"fetchActiveItems\",\"outputs\": [{\"components\": [{\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"address payable\",\"name\": \"seller\",\"type\": \"address\"},{\"internalType\": \"address payable\",\"name\": \"buyer\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"internalType\": \"struct NFTMarketplace.MarketItem[]\",\"name\": \"\",\"type\": \"tuple[]\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"fetchMyCreatedItems\",\"outputs\": [{\"components\": [{\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"address payable\",\"name\": \"seller\",\"type\": \"address\"},{\"internalType\": \"address payable\",\"name\": \"buyer\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"internalType\": \"struct NFTMarketplace.MarketItem[]\",\"name\": \"\",\"type\": \"tuple[]\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"fetchMyPurchasedItems\",\"outputs\": [{\"components\": [{\"internalType\": \"uint256\",\"name\": \"id\",\"type\": \"uint256\"},{\"internalType\": \"address\",\"name\": \"nftContract\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"tokenId\",\"type\": \"uint256\"},{\"internalType\": \"address payable\",\"name\": \"seller\",\"type\": \"address\"},{\"internalType\": \"address payable\",\"name\": \"buyer\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"price\",\"type\": \"uint256\"},{\"internalType\": \"enum NFTMarketplace.State\",\"name\": \"state\",\"type\": \"uint8\"}],\"internalType\": \"struct NFTMarketplace.MarketItem[]\",\"name\": \"\",\"type\": \"tuple[]\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"getListingFee\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"listingFee\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"marketowner\",\"outputs\": [{\"internalType\": \"address payable\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"}]";
    private string nftAbi = "[{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"previousOwner\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"newOwner\",\"type\": \"address\"}],\"name\": \"OwnershipTransferred\",\"type\": \"event\"},{\"inputs\": [],\"name\": \"owner\",\"outputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"renounceOwnership\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"newOwner\",\"type\": \"address\"}],\"name\": \"transferOwnership\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"}]";

    public async void buyItem(TextMeshProUGUI tokenId)
    {
        string value = "1000000000000000000";

        // array of arguments for contract
        string args = "[\"" + nftContractAddress + "\", \"" + tokenId.text + "\"]";
        
        //string chainId = await EVM.ChainId(chain, network, mumbaiRpc);
        //string gasPrice = await EVM.GasPrice(chain, network, mumbaiRpc);
        //string data = await EVM.CreateContractData(abi, sellItemMethod, args);
        //string gasLimit = "75000";
        //string transaction = await EVM.CreateTransaction(chain, network, Manager.PlayerAddress, marketplaceContractAddress, value, data, gasPrice, gasLimit, mumbaiRpc);
        //Debug.Log("selling... " + Manager.PlayerPK + " " +  transaction + " " + chainId);
        //string signature = Web3PrivateKey.SignTransaction(Manager.PlayerPK, transaction, chainId);
        //string response = await EVM.BroadcastTransaction(chain, network, Manager.PlayerAddress, marketplaceContractAddress, value, data, signature, gasPrice, gasLimit, mumbaiRpc);


       // Debug.Log("item sold! " + response);
    }


    public async void listForSale(string tokenId, string price)
    {
        //// first we need to approve the nft, for that we give the permission to the marketplace to sell the nft on the behalf of the owner
        //string args = "[\"" + marketplaceContractAddress + "\", \"" + tokenId + "\"]";
        //string approveResponse = await EVM.Call(chain, network, nftContractAddress, nftAbi, approveMethod, args, mumbaiRpc);
        //Debug.Log("We gave permission to the marketplace to list our item and sell on our behalf. " + approveResponse);

        //// now we can list for sale
        //string sellItemArgs = "[\"" + nftContractAddress + "\", \"" + tokenId + "\", \"" + price + "]";
        //string listResponse = await EVM.Call(chain, network, marketplaceContractAddress, abi, createSaleItemMethod, sellItemArgs, mumbaiRpc);

        //Debug.Log("item listed for sale! " + listResponse);
    }

    public async void removeFromMarket(TextMeshProUGUI tokenId)
    {
        // fetchMyCreatedItems
        //string args = "[]";
        //string listResponse = await EVM.Call(chain, network, marketplaceContractAddress, abi, fetchListedItemsMethod, args, mumbaiRpc);
        //var result = JsonConvert.DeserializeObject<List<List<string>>>(listResponse);
        //Debug.Log("fetched my list of market items: " + result.Count);
        //List<MarketItem> marketItems = new List<MarketItem>();
        //// loop through, find matching tokenId
        //foreach (var currentMarketItem in result)
        //{
        //    int currentTokenId = int.Parse(currentMarketItem[2]);
        //    if (currentTokenId.ToString().Equals(tokenId.text))
        //    {
        //        // deleteMarketItem
        //        string deleteArgs = "[\"" + tokenId.text + "\"]";
        //        string approveResponse = await EVM.Call(chain, network, nftContractAddress, nftAbi, deleteSaleItemMethod, deleteArgs, mumbaiRpc);
        //        Debug.Log("removed item from market! " + approveResponse);
        //    }
        //} 
    }

}

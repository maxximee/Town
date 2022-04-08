using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CustomCallExample : MonoBehaviour
{
    public string MarketplaceContractAddress = "0x37A60BE4309FC071ec219C949682cF1A785C2f81";
    public string mumbaiRpc = "https://rpc-mumbai.matic.today";

    public UnityEvent marketLoadedEvent;

    async void Start()
    {
        /*
        // SPDX-License-Identifier: MIT
        pragma solidity ^0.8.0;

        contract AddTotal {
            uint256 public myTotal = 0;

            function addTotal(uint8 _myArg) public {
                myTotal = myTotal + _myArg;
            }
        }
        */
        // set chain: ethereum, moonbeam, polygon etc
        string chain = "polygon";
        // set network mainnet, testnet
        string network = "mumbai";
        // smart contract method to call
        string method = "fetchActiveItems";
        // abi in json format
        string abi = "[	{ \"inputs\": [ { \"internalType\": \"address\", \"name\": \"nftContract\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"price\", \"type\": \"uint256\" } ], \"name\": \"createMarketItem\", \"outputs\": [], \"stateMutability\": \"payable\", \"type\": \"function\"	},	{ \"inputs\": [ { \"internalType\": \"address\", \"name\": \"nftContract\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" } ], \"name\": \"createMarketSale\", \"outputs\": [], \"stateMutability\": \"payable\", \"type\": \"function\"	},	{ \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"itemId\", \"type\": \"uint256\" } ], \"name\": \"deleteMarketItem\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\"	},	{ \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\"	},	{ \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"nftContract\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"indexed\": false, \"internalType\": \"address\", \"name\": \"seller\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"address\", \"name\": \"buyer\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"price\", \"type\": \"uint256\" }, { \"indexed\": false, \"internalType\": \"enum NFTMarketplace.State\", \"name\": \"state\", \"type\": \"uint8\" } ], \"name\": \"MarketItemCreated\", \"type\": \"event\"	},	{ \"anonymous\": false, \"inputs\": [ { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"id\", \"type\": \"uint256\" }, { \"indexed\": true, \"internalType\": \"address\", \"name\": \"nftContract\", \"type\": \"address\" }, { \"indexed\": true, \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"indexed\": false, \"internalType\": \"address\", \"name\": \"seller\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"address\", \"name\": \"buyer\", \"type\": \"address\" }, { \"indexed\": false, \"internalType\": \"uint256\", \"name\": \"price\", \"type\": \"uint256\" }, { \"indexed\": false, \"internalType\": \"enum NFTMarketplace.State\", \"name\": \"state\", \"type\": \"uint8\" } ], \"name\": \"MarketItemSold\", \"type\": \"event\"	},	{ \"inputs\": [], \"name\": \"fetchActiveItems\", \"outputs\": [ { \"components\": [ 	{  \"internalType\": \"uint256\",  \"name\": \"id\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"address\",  \"name\": \"nftContract\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"uint256\",  \"name\": \"tokenId\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"address payable\",  \"name\": \"seller\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"address payable\",  \"name\": \"buyer\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"uint256\",  \"name\": \"price\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"enum NFTMarketplace.State\",  \"name\": \"state\",  \"type\": \"uint8\" 	} ], \"internalType\": \"struct NFTMarketplace.MarketItem[]\", \"name\": \"\", \"type\": \"tuple[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\"	},	{ \"inputs\": [], \"name\": \"fetchMyCreatedItems\", \"outputs\": [ { \"components\": [ 	{  \"internalType\": \"uint256\",  \"name\": \"id\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"address\",  \"name\": \"nftContract\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"uint256\",  \"name\": \"tokenId\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"address payable\",  \"name\": \"seller\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"address payable\",  \"name\": \"buyer\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"uint256\",  \"name\": \"price\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"enum NFTMarketplace.State\",  \"name\": \"state\",  \"type\": \"uint8\" 	} ], \"internalType\": \"struct NFTMarketplace.MarketItem[]\", \"name\": \"\", \"type\": \"tuple[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\"	},	{ \"inputs\": [], \"name\": \"fetchMyPurchasedItems\", \"outputs\": [ { \"components\": [ 	{  \"internalType\": \"uint256\",  \"name\": \"id\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"address\",  \"name\": \"nftContract\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"uint256\",  \"name\": \"tokenId\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"address payable\",  \"name\": \"seller\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"address payable\",  \"name\": \"buyer\",  \"type\": \"address\" 	}, 	{  \"internalType\": \"uint256\",  \"name\": \"price\",  \"type\": \"uint256\" 	}, 	{  \"internalType\": \"enum NFTMarketplace.State\",  \"name\": \"state\",  \"type\": \"uint8\" 	} ], \"internalType\": \"struct NFTMarketplace.MarketItem[]\", \"name\": \"\", \"type\": \"tuple[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\"	},	{ \"inputs\": [], \"name\": \"getListingFee\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\"	},	{ \"inputs\": [], \"name\": \"listingFee\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"marketowner\", \"outputs\": [ { \"internalType\": \"address payable\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }]";
        // address of contract
        string contract = MarketplaceContractAddress;
        // array of arguments for contract
        string args = "[]";
        // connects to user's browser wallet to call a transaction
        string response = await EVM.Call(chain, network, contract, abi, method, args, mumbaiRpc);
        // display response in game
        var result = JsonConvert.DeserializeObject<List<List<string>>>(response);
        List<MarketItem> marketItems = new List<MarketItem>();
        foreach (var currentMarketItem in result)
        {
            MarketItem item = new MarketItem();
            item.Id = int.Parse(currentMarketItem[0]);
            item.ContractAddress = currentMarketItem[1];
            item.TokenId = int.Parse(currentMarketItem[2]);
            item.SellerAddress = currentMarketItem[3];
            item.BuyerAddress = currentMarketItem[4];
            item.Price = int.Parse(currentMarketItem[5]);
            item.State = int.Parse(currentMarketItem[6]);
            marketItems.Add(item);
        }
        Manager.SetMarketItems(marketItems);
        Debug.Log("market data loaded, invoking event");
        marketLoadedEvent.Invoke();

    }

}

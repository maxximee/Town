using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketItemDeprecated
{
    private int id;
    private string contractAddress;
    private int tokenId;
    private string sellerAddress;
    private string buyerAddress;
    private int price;
    private int state;

    public int Id { get => id; set => id = value; }
    public string ContractAddress { get => contractAddress; set => contractAddress = value; }
    public int TokenId { get => tokenId; set => tokenId = value; }
    public string SellerAddress { get => sellerAddress; set => sellerAddress = value; }
    public string BuyerAddress { get => buyerAddress; set => buyerAddress = value; }
    public int Price { get => price; set => price = value; }
    public int State { get => state; set => state = value; }
}
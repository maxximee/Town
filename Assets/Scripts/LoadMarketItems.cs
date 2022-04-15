using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using static LoadPlayerNfts;

public class LoadMarketItems : MonoBehaviour
{


    public UnityEvent marketLoadedEvent;

    public partial class FetchActiveItemsOutputDTO : FetchActiveItemsOutputDTOBase { }

    [FunctionOutput]
    public class FetchActiveItemsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "", 1)]
        public virtual List<MarketItem> ReturnValue1 { get; set; }
    }

    public partial class FetchMyCreatedItemsOutputDTO : FetchMyCreatedItemsOutputDTOBase { }

    [FunctionOutput]
    public class FetchMyCreatedItemsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "", 1)]
        public virtual List<MarketItem> ReturnValue1 { get; set; }
    }

    public partial class FetchMyPurchasedItemsOutputDTO : FetchMyPurchasedItemsOutputDTOBase { }

    [FunctionOutput]
    public class FetchMyPurchasedItemsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple[]", "", 1)]
        public virtual List<MarketItem> ReturnValue1 { get; set; }
    }

    public partial class GetListingFeeOutputDTO : GetListingFeeOutputDTOBase { }

    [FunctionOutput]
    public class GetListingFeeOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ListingFeeOutputDTO : ListingFeeOutputDTOBase { }

    [FunctionOutput]
    public class ListingFeeOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class MarketownerOutputDTO : MarketownerOutputDTOBase { }

    [FunctionOutput]
    public class MarketownerOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class MarketItem : MarketItemBase { }

    public class MarketItemBase
    {
        [Parameter("uint256", "id", 1)]
        public virtual BigInteger Id { get; set; }
        [Parameter("address", "nftContract", 2)]
        public virtual string NftContract { get; set; }
        [Parameter("uint256", "tokenId", 3)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("address", "seller", 4)]
        public virtual string Seller { get; set; }
        [Parameter("address", "buyer", 5)]
        public virtual string Buyer { get; set; }
        [Parameter("uint256", "price", 6)]
        public virtual BigInteger Price { get; set; }
        [Parameter("uint8", "state", 7)]
        public virtual byte State { get; set; }
    }

    public partial class FetchActiveItemsFunction : FetchActiveItemsFunctionBase { }

    [Function("fetchActiveItems", typeof(FetchActiveItemsOutputDTO))]
    public class FetchActiveItemsFunctionBase : FunctionMessage
    {

    }

    public partial class CreateMarketItemFunction : CreateMarketItemFunctionBase { }

    [Function("createMarketItem")]
    public class CreateMarketItemFunctionBase : FunctionMessage
    {
        [Parameter("address", "nftContract", 1)]
        public virtual string NftContract { get; set; }
        [Parameter("uint256", "tokenId", 2)]
        public virtual BigInteger TokenId { get; set; }
        [Parameter("uint256", "price", 3)]
        public virtual BigInteger Price { get; set; }
    }

    public partial class CreateMarketSaleFunction : CreateMarketSaleFunctionBase { }

    [Function("createMarketSale")]
    public class CreateMarketSaleFunctionBase : FunctionMessage
    {
        [Parameter("address", "nftContract", 1)]
        public virtual string NftContract { get; set; }
        [Parameter("uint256", "id", 2)]
        public virtual BigInteger Id { get; set; }

    }

    public partial class DeleteMarketItemFunction : DeleteMarketItemFunctionBase { }

    [Function("deleteMarketItem")]
    public class DeleteMarketItemFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "itemId", 1)]
        public virtual BigInteger ItemId { get; set; }
    }


    async void Start()
    {

        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.PlayerPK;
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);
        var contractHandler = web3.Eth.GetContractHandler(Manager.MarketplaceContractAddress);

        var query = await contractHandler.QueryDeserializingToObjectAsync<FetchActiveItemsFunction, FetchActiveItemsOutputDTO>();

        Manager.SetMarketItems(query.ReturnValue1);
        Debug.Log("market data loaded, invoking event");
        marketLoadedEvent.Invoke();

    }

    public async void buyItem(BigInteger id, BigInteger price)
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.PlayerPK;
        var account = new Account(privateKey, Manager.ChainId);
        var web3 = new Web3(account, url);
        var contractHandler = web3.Eth.GetContractTransactionHandler<CreateMarketSaleFunction>();
        var createMarketSaleFunction = new CreateMarketSaleFunction() {
            NftContract = Manager.NftContractAddress,
            Id = id
        };
        createMarketSaleFunction.AmountToSend = price;
        var createMarketSaleFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(Manager.MarketplaceContractAddress, createMarketSaleFunction);

        Debug.Log("item bought! " + createMarketSaleFunctionTxnReceipt.TransactionHash);
    }

    public async void removeItem(BigInteger itemId)
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.PlayerPK;
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);
        var contractHandler = web3.Eth.GetContractHandler(Manager.MarketplaceContractAddress);

        var deleteMarketItemFunction = new DeleteMarketItemFunction();
        deleteMarketItemFunction.ItemId = itemId;
        var deleteMarketItemFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(deleteMarketItemFunction);

        Debug.Log("item removed from market! " + deleteMarketItemFunctionTxnReceipt);
    }

    public async void listItemForSale(BigInteger tokenId, BigInteger price)
    {
        // TODO nft.approve first!

        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.PlayerPK;
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);
        var contractHandler = web3.Eth.GetContractHandler(Manager.MarketplaceContractAddress);

        var createMarketItemFunction = new CreateMarketItemFunction();
        createMarketItemFunction.NftContract = Manager.NftContractAddress;
        createMarketItemFunction.TokenId = tokenId;
        createMarketItemFunction.Price = price;
        var createMarketItemFunctionTxnReceipt = await contractHandler.SendRequestAndWaitForReceiptAsync(createMarketItemFunction);

        Debug.Log("item listed! " + createMarketItemFunctionTxnReceipt);
    }
}

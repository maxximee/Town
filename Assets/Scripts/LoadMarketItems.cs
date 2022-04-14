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

    async void Start()
    {

        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.BankPK;
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);
        var contract = web3.Eth.GetContract(Manager.marketAbi, Manager.MarketplaceContractAddress);
//        var function = contract.GetFunction("fetchActiveItems");
        var contractHandler = web3.Eth.GetContractHandler(Manager.MarketplaceContractAddress);

        var query = await contractHandler.QueryDeserializingToObjectAsync<FetchActiveItemsFunction, FetchActiveItemsOutputDTO>();

        Manager.SetMarketItems(query.ReturnValue1);
        Debug.Log("market data loaded, invoking event");
        marketLoadedEvent.Invoke();

        //        var response = await function.CallAsync<List<List<string>>>();

        //var result = JsonConvert.DeserializeObject<List<List<string>>>(response);
        //List<MarketItem> marketItems = new List<MarketItem>();
        //foreach (var currentMarketItem in response)
        //{
        //    Debug.Log("item " + currentMarketItem);
        //    Debug.Log("item " + currentMarketItem[0]);

        //    MarketItem item = new MarketItem();
        //    item.Id = int.Parse(currentMarketItem[0]);
        //    item.ContractAddress = currentMarketItem[1];
        //    item.TokenId = int.Parse(currentMarketItem[2]);
        //    item.SellerAddress = currentMarketItem[3];
        //    item.BuyerAddress = currentMarketItem[4];
        //    item.Price = int.Parse(currentMarketItem[5]);
        //    item.State = int.Parse(currentMarketItem[6]);
        //    marketItems.Add(item);
        //}
        //Manager.SetMarketItems(marketItems);
        //Debug.Log("market data loaded, invoking event");
        //marketLoadedEvent.Invoke();
    }


}

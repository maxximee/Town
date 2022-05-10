using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Nethereum.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using System.Threading.Tasks;
using System.Numerics;

public class LoadPlayerNfts : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField] private GameObject dragonPanel;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI topSpeedValue;
    [SerializeField] private Slider topSpeedSlider;
    [SerializeField] private TextMeshProUGUI accelValue;
    [SerializeField] private Slider accelSlider;

    [SerializeField] private TextMeshProUGUI yieldValue;
    [SerializeField] private Slider yieldSlider;
    [SerializeField] private TextMeshProUGUI dietValue;
    [SerializeField] private Slider dietSlider;

    [SerializeField] private TextMeshProUGUI levelValue;

    [SerializeField] private TextMeshProUGUI rarityValue;
    [SerializeField] private Image raceSprite;
    [SerializeField] private Sprite fireRace;
    [SerializeField] private Sprite waterRace;
    [SerializeField] private Image dragonSprite;
    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private TextMeshProUGUI dragTokenId;

    [Header("3D Elements")]
    [SerializeField] private GameObject[] dragonPrefabs;
    [SerializeField] private GameObject dragonInitPost;

    private string[] allTokenIds;
    private int currentIndex = 0;


    [Function("tokenURI", "string")]
    public class TokenURIFunction : FunctionMessage
    {
        [Parameter("uint256", "_tokenId", 1)]
        public int TokenId { get; set; }
    }

    public partial class OwnerOfFunction : OwnerOfFunctionBase { }

    [Function("ownerOf", "address")]
    public class OwnerOfFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "tokenId", 1)]
        public virtual System.Numerics.BigInteger TokenId { get; set; }
    }

    public partial class OwnerOfOutputDTO : OwnerOfOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOfOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "owner", 1)]
        public virtual string Owner { get; set; }
    }

    public partial class TokenDetailsFunction : TokenDetailsFunctionBase { }

    [Function("_tokenDetails", typeof(TokenDetailsOutputDTO))]
    public class TokenDetailsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetTokenDetailsOutputDTO : GetTokenDetailsOutputDTOBase { }

    [FunctionOutput]
    public class GetTokenDetailsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("tuple", "", 1)]
        public virtual Dragon ReturnValue1 { get; set; }
    }

    public partial class Dragon : DragonBase { }

    public class DragonBase
    {
        [Parameter("uint256", "id", 1)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "bloodType", 2)]
        public virtual BigInteger BloodType { get; set; }
        [Parameter("uint256", "topSpeed", 3)]
        public virtual BigInteger TopSpeed { get; set; }
        [Parameter("uint256", "acceleration", 4)]
        public virtual BigInteger Acceleration { get; set; }
        [Parameter("uint256", "yield", 5)]
        public virtual BigInteger Yield { get; set; }
        [Parameter("uint256", "diet", 6)]
        public virtual BigInteger Diet { get; set; }
        [Parameter("uint8", "level", 7)]
        public virtual byte Level { get; set; }
        [Parameter("string", "rarity", 8)]
        public virtual string Rarity { get; set; }
        [Parameter("string", "tokenURI", 9)]
        public virtual string TokenURI { get; set; }
    }

    public partial class GetTokenDetailsFunction : GetTokenDetailsFunctionBase { }

    [Function("getTokenDetails", typeof(GetTokenDetailsOutputDTO))]
    public class GetTokenDetailsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_id", 1)]
        public virtual BigInteger Id { get; set; }
    }

    public partial class TokenDetailsOutputDTO : TokenDetailsOutputDTOBase { }

    [FunctionOutput]
    public class TokenDetailsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "id", 1)]
        public virtual BigInteger Id { get; set; }
        [Parameter("uint256", "bloodType", 2)]
        public virtual BigInteger BloodType { get; set; }
        [Parameter("uint256", "topSpeed", 3)]
        public virtual BigInteger TopSpeed { get; set; }
        [Parameter("uint256", "acceleration", 4)]
        public virtual BigInteger Acceleration { get; set; }
        [Parameter("uint256", "yield", 5)]
        public virtual BigInteger Yield { get; set; }
        [Parameter("uint256", "diet", 6)]
        public virtual BigInteger Diet { get; set; }
        [Parameter("uint8", "level", 7)]
        public virtual byte Level { get; set; }
        [Parameter("string", "rarity", 8)]
        public virtual string Rarity { get; set; }
        [Parameter("string", "tokenURI", 9)]
        public virtual string TokenURI { get; set; }
    }

    public partial class GetDragonsByOwnerFunction : GetDragonsByOwnerFunctionBase { }

    [Function("getDragonsByOwner", "uint256[]")]
    public class GetDragonsByOwnerFunctionBase : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public virtual string Owner { get; set; }
    }




    void Start()
    {
        if (PlayerPrefs.HasKey("SelectedDragonTokenId"))
        {
            SelectDragon(PlayerPrefs.GetString("SelectedDragonTokenId"));
            Manager.setSelectedDragon(PlayerPrefs.GetString("SelectedDragonTokenId"));
        }

        indexText.text = "0 of 0";
        if (!String.IsNullOrEmpty(Manager.PlayerPK))
        {
            LoadInitially();
            if (PlayerPrefs.HasKey("tokenIds"))
            {
                allTokenIds = PlayerPrefs.GetString("tokenIds").Split(',');
                if (allTokenIds.Length > 0)
                {
                    LoadDragon(int.Parse(allTokenIds[currentIndex]));
                    indexText.text = "1 of " + allTokenIds.Length;
                }

                BalanceOfPlayer();

                Button but = selectButton.GetComponent<Button>();
                selectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Select";
                but.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("go buy a dragon");
            }
        }
    }

    public async void BalanceOfPlayer()
    {
        if (!String.IsNullOrEmpty(Manager.PlayerPK))
        {
            var url = Manager.infuraMumbaiEndpointUrl;
            var privateKey = Manager.PlayerPK;
            var account = new Account(privateKey);
            var web3 = new Web3(account, url);
            var contractHandler = web3.Eth.GetContractHandler(Manager.NftContractAddress);
            var balanceOfFunction = new BalanceOfFunction();
            balanceOfFunction.Owner = Manager.PlayerAddress;
            var balanceOfFunctionReturn = await contractHandler.QueryAsync<BalanceOfFunction, System.Numerics.BigInteger>(balanceOfFunction);
            Debug.Log("user balance of dragons is: " + balanceOfFunctionReturn.ToString());
            Manager.playerAmountOfDragons = balanceOfFunctionReturn;
        }
    }

    async void LoadInitially()
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.PlayerPK;
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);
        var contractHandler = web3.Eth.GetContractHandler(Manager.NftContractAddress);
        string myDragons = "";

        var getDragonsByOwnerFunction = new GetDragonsByOwnerFunction();
        getDragonsByOwnerFunction.Owner = account.Address;
        var getDragonsByOwnerFunctionReturn = await contractHandler.QueryAsync<GetDragonsByOwnerFunction, List<BigInteger>>(getDragonsByOwnerFunction);
        foreach (BigInteger tokenId in getDragonsByOwnerFunctionReturn)
        {
            myDragons += tokenId + ",";
        }

        if (myDragons.Length > 1)
        {
            PlayerPrefs.SetString("tokenIds", myDragons.Remove(myDragons.Length - 1, 1));
        }
    }

    async void LoadDragon(int tokenId)
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var web3 = new Web3(url);

        var tokenURIFunctionMessage = new TokenURIFunction()
        {
            TokenId = tokenId,
        };

        var contractHandler = web3.Eth.GetContractHandler(Manager.NftContractAddress);
        var getTokenDetailsFunction = new GetTokenDetailsFunction();
        getTokenDetailsFunction.Id = tokenId;
        var getTokenDetailsOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetTokenDetailsFunction, GetTokenDetailsOutputDTO>(getTokenDetailsFunction);

        Dragon dragon = getTokenDetailsOutputDTO.ReturnValue1;
        string uri = dragon.TokenURI;
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();
        NftDragonData data = JsonUtility.FromJson<NftDragonData>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

        // parse json to get image uri (even if we won't use it in here)
        string imageUri = data.image;
        name.text = data._id.ToString();

        dragTokenId.text = tokenId.ToString();

        topSpeedValue.text = (((float)dragon.TopSpeed) / 10).ToString();
        topSpeedSlider.value = ((float)dragon.TopSpeed) / 10;

        accelValue.text = (((float)dragon.Acceleration) / 10).ToString();
        accelSlider.value = ((float)dragon.Acceleration) / 10;

        dietValue.text = (((float)dragon.Diet) / 10).ToString();
        dietSlider.value = ((float)dragon.Diet) / 10;

        yieldValue.text = (((float)dragon.Yield) / 10).ToString();
        yieldSlider.value = ((float)dragon.Yield) / 10;

        levelValue.text = dragon.Level.ToString();

        rarityValue.text = dragon.Rarity.ToString();

        if (data.classType.Equals("fire"))
        {
            raceSprite.sprite = fireRace;
            raceSprite.enabled = true;
        }

        // This is not needed anymore, we don't have to load the image, we load the 3D model!
        // // fetch image and display in game
        // UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        // await textureRequest.SendWebRequest();
        // dragonSprite.sprite = Sprite.Create(DownloadHandlerTexture.GetContent(textureRequest), new Rect(0, 0, 1023, 1023), new UnityEngine.Vector2(0.5f, 1f), 100f);
    }

    public void LoadNextDragon()
    {

        if (currentIndex == allTokenIds.Length - 1)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }
        indexText.text = (currentIndex + 1) + " of " + allTokenIds.Length;
        LoadDragon(int.Parse(allTokenIds[currentIndex]));
    }

    public void LoadPreviousDragon()
    {

        if (currentIndex == 0)
        {
            currentIndex = allTokenIds.Length - 1;
        }
        else
        {
            currentIndex--;
        }
        indexText.text = (currentIndex + 1) + " of " + allTokenIds.Length;
        LoadDragon(int.Parse(allTokenIds[currentIndex]));
    }

    public void SelectDragon()
    {
        SelectDragon(allTokenIds[currentIndex]);
    }

    public void SelectDragon(string index)
    {
        foreach (Transform child in dragonInitPost.transform)
        {
            if (child.gameObject.tag.Equals("IntroDragon"))
            {
                Destroy(child.gameObject);
            }
        }
        Manager.setSelectedDragon(index);
        PlayerPrefs.SetString("SelectedDragonTokenId", index);
        // GameObject newDragon = Instantiate(dragonPrefabs[int.Parse(index)]);
        string dragonName;
        if (int.Parse(index) < 10)
        {
            dragonName = "DragonSD_0" + index;
        }
        else
        {
            dragonName = "DragonSD_" + index;
        }
        GameObject newDragon = Instantiate(Resources.Load("Dragons_SD/Prefab/" + dragonName) as GameObject);
        newDragon.transform.parent = dragonInitPost.transform;
        newDragon.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
        newDragon.transform.localRotation = UnityEngine.Quaternion.identity;
    }

}

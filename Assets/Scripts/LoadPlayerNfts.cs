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
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI bloodTypeText;
    [SerializeField] private TextMeshProUGUI topSpeedValue;
    [SerializeField] private TextMeshProUGUI accelValue;
    [SerializeField] private TextMeshProUGUI yieldValue;
    [SerializeField] private TextMeshProUGUI dietValue;
    [SerializeField] private TextMeshProUGUI levelValue;

    [SerializeField] private TextMeshProUGUI rarityValue;
    [SerializeField] private Image raceSprite;
    [SerializeField] private Sprite fireRace;
    [SerializeField] private Sprite frostRace;

    [SerializeField] private Sprite mysticRace;
    [SerializeField] private Sprite exoticRace;
    [SerializeField] private Image dragonSprite;

    [SerializeField] private Transform dragonLoadPoint;
    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private TextMeshProUGUI dragTokenId;

    [SerializeField] private Sprite commonFrame;
    [SerializeField] private Sprite uncommonFrame;
    [SerializeField] private Sprite rareFrame;
    [SerializeField] private Sprite epicFrame;

    [SerializeField] private Image rarityFrame;

    [Header("3D Elements")]
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
        string currentSelectedDragonTokenId = "0";
        if (PlayerPrefs.HasKey("SelectedDragonTokenId"))
        {
            SelectDragon(PlayerPrefs.GetString("SelectedDragonTokenId"));
            Manager.setSelectedDragon(PlayerPrefs.GetString("SelectedDragonTokenId"));
            currentSelectedDragonTokenId = PlayerPrefs.GetString("SelectedDragonTokenId");
        }

        indexText.text = "0 of 0";
        if (!String.IsNullOrEmpty(Manager.PlayerAddress))
        {
            LoadInitially();
            if (PlayerPrefs.HasKey("tokenIds"))
            {
                allTokenIds = PlayerPrefs.GetString("tokenIds").Split(',');
                currentIndex = getIndex(currentSelectedDragonTokenId);
                if (allTokenIds.Length > 0)
                {
                    LoadDragon(int.Parse(allTokenIds[currentIndex]));
                    indexText.text = (currentIndex + 1) + " of " + allTokenIds.Length;
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

    private int getIndex(string tokenId)
    {
        for (int i = 0; i <= allTokenIds.Length; i++)
        {
            if (allTokenIds[i].Equals(tokenId))
            {
                return i;
            }
        }
        return 0;
    }

    public async void BalanceOfPlayer()
    {
        if (!String.IsNullOrEmpty(Manager.PlayerAddress))
        {
            var url = Manager.infuraMumbaiEndpointUrl;
            var web3 = new Web3(url);
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
        var web3 = new Web3(url);
        var contractHandler = web3.Eth.GetContractHandler(Manager.NftContractAddress);
        string myDragons = "";

        var getDragonsByOwnerFunction = new GetDragonsByOwnerFunction();
        getDragonsByOwnerFunction.Owner = Manager.PlayerAddress;
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

    public async Task<Dragon> LoadDragon(string tokenId)
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var web3 = new Web3(url);

        var tokenURIFunctionMessage = new TokenURIFunction()
        {
            TokenId = int.Parse(tokenId),
        };

        var contractHandler = web3.Eth.GetContractHandler(Manager.NftContractAddress);
        var getTokenDetailsFunction = new GetTokenDetailsFunction();
        getTokenDetailsFunction.Id = int.Parse(tokenId);
        var getTokenDetailsOutputDTO = await contractHandler.QueryDeserializingToObjectAsync<GetTokenDetailsFunction, GetTokenDetailsOutputDTO>(getTokenDetailsFunction);

        Dragon dragon = getTokenDetailsOutputDTO.ReturnValue1;
        return dragon;
    }

    public async Task<NftDragonData> LoadDragonData(Dragon dragon)
    {
        string uri = dragon.TokenURI;
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();
        NftDragonData data = JsonUtility.FromJson<NftDragonData>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        return data;
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

        name.text = "DRAGON #0" + data._id.ToString();

        dragTokenId.text = tokenId.ToString();

        topSpeedValue.text = (((float)dragon.TopSpeed) / 10).ToString();

        accelValue.text = (((float)dragon.Acceleration) / 10).ToString();

        dietValue.text = (((float)dragon.Diet) / 10).ToString();

        yieldValue.text = (((float)dragon.Yield) / 10).ToString();

        levelValue.text = dragon.Level.ToString();

        Manager.addDragon(new DragonDataModel(dragon.TopSpeed, dragon.Acceleration, dragon.Yield, dragon.Diet), (currentIndex + 1).ToString());

        string rarityString = dragon.Rarity.ToString().ToUpper();
        rarityValue.text = rarityString;

        switch (rarityString)
        {
            case "COMMON":
                rarityFrame.sprite = commonFrame;
                break;
            case "UNCOMMON":
                rarityFrame.sprite = uncommonFrame;
                break;
            case "RARE":
                rarityFrame.sprite = rareFrame;
                break;
            case "EPIC":
                rarityFrame.sprite = epicFrame;
                break;
            default:
                rarityFrame.sprite = commonFrame;
                break;
        }

        bloodTypeText.text = "Blood type " + dragon.BloodType.ToString();

        expSlider.value = 29;

        switch (data.classType)
        {
            case "fire":
                raceSprite.sprite = fireRace;
                break;
            case "frost":
                raceSprite.sprite = frostRace;
                break;
            case "mystic":
                raceSprite.sprite = mysticRace;
                break;
            case "exotic":
                raceSprite.sprite = exoticRace;
                break;
        }

        // This is not needed anymore, we don't have to load the image from web, it's too slow //
        // string imageUri = data.image;
        // // fetch image and display in game
        // UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        // await textureRequest.SendWebRequest();
        // dragonSprite.sprite = Sprite.Create(DownloadHandlerTexture.GetContent(textureRequest), new Rect(0, 0, 1023, 1023), new UnityEngine.Vector2(0.5f, 1f), 100f);

        // replaced with 3d dragon
        // string spriteNamePrefix = "DragonSD_";
        // if (tokenId < 10)
        // {
        //     spriteNamePrefix = spriteNamePrefix + "0";
        // }
        // Sprite sp = Resources.Load<Sprite>("Dragons_Sprites/" + spriteNamePrefix + tokenId.ToString());
        // dragonSprite.sprite = sp;

        foreach (Transform child in dragonLoadPoint.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }

        // TODO tokenId won't work once we have more tokens than unique dragon. Should be bloodType
        string spriteNamePrefix = "DragonSD_";
        if (tokenId < 10)
        {
            spriteNamePrefix = spriteNamePrefix + "0";
        }
        GameObject DragonUI3d = Instantiate(Resources.Load("Dragons_SD/Prefab/" + spriteNamePrefix + tokenId.ToString()) as GameObject);
        // set UI layer
        SetLayerRecursively(DragonUI3d, 5);
        DragonUI3d.transform.parent = dragonLoadPoint;
        DragonUI3d.transform.localRotation = UnityEngine.Quaternion.identity;
        DragonUI3d.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
        DragonUI3d.transform.localScale = new UnityEngine.Vector3(1, 1, 1);

    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
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

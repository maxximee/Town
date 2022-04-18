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

public class LoadPlayerNfts : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI topSpeedValue;
    [SerializeField] private Slider topSpeedSlider;
    [SerializeField] private TextMeshProUGUI accelValue;
    [SerializeField] private Slider accelSlider;
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

    void Start()
    {
        if (!PlayerPrefs.HasKey("tokenIds"))
        {
            LoadInitially();
        }

        allTokenIds = PlayerPrefs.GetString("tokenIds").Split(',');
        if (allTokenIds.Length > 0) {
            LoadDragon(int.Parse(allTokenIds[currentIndex]));
        } else {
            // show buy first dragon
        }

        BalanceOfPlayer();
    }

    public async void BalanceOfPlayer() {
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

    async void LoadInitially() {
        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.PlayerPK;
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);
        var contractHandler = web3.Eth.GetContractHandler(Manager.NftContractAddress);
        string myDragons = "";
        // TODO only 4 dragons for now proto
        for (int i = 0; i<4;i++) {
            var ownerOfFunction = new OwnerOfFunction();
            ownerOfFunction.TokenId = i;
            var ownerOfFunctionReturn = await contractHandler.QueryAsync<OwnerOfFunction, string>(ownerOfFunction);
            Debug.Log("owner of " + i + " is " + ownerOfFunctionReturn);
            if (ownerOfFunctionReturn.Equals(Manager.PlayerAddress)) {
                myDragons += i + ",";
            }
        }
        if (myDragons.Length > 1) {
            PlayerPrefs.SetString("tokenIds", myDragons.Remove(myDragons.Length - 1, 1));
        }
    }

    async void LoadDragon(int tokenId)
    {
        var url = Manager.infuraMumbaiEndpointUrl;
        var privateKey = Manager.PlayerPK;
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);

        var tokenURIFunctionMessage = new TokenURIFunction()
        {
            TokenId = tokenId,
        };

        var queryHandler = web3.Eth.GetContractQueryHandler<TokenURIFunction>();
        string uri = await queryHandler.QueryAsync<string>(Manager.NftContractAddress, tokenURIFunctionMessage);

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();
        NftResponseData data = JsonUtility.FromJson<NftResponseData>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

        // parse json to get image uri
        string imageUri = data.image;
        name.text = data.name;

        dragTokenId.text = tokenId.ToString();

        topSpeedValue.text = data.topSpeed.ToString();
        topSpeedSlider.value = data.topSpeed;

        accelValue.text = data.acceleration.ToString();
        accelSlider.value = data.acceleration;

        if (data.race.Equals("fire"))
        {
            raceSprite.sprite = fireRace;
            raceSprite.enabled = true;
        }

        description.text = data.description;

        // fetch image and display in game
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        await textureRequest.SendWebRequest();
        dragonSprite.sprite = Sprite.Create(((DownloadHandlerTexture)textureRequest.downloadHandler).texture, new Rect(0, 0, 1233, 1233), new Vector2(0.5f, 1f), 100f);
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
        foreach (Transform child in dragonInitPost.transform)
        {
            if (child.gameObject.tag.Equals("IntroDragon"))
            {
                Destroy(child.gameObject);
            }
        }
        Manager.setSelectedDragon(allTokenIds[currentIndex]);
        GameObject newDragon = Instantiate(dragonPrefabs[int.Parse(allTokenIds[currentIndex])]);
        newDragon.transform.parent = dragonInitPost.transform;
        newDragon.transform.localPosition = new Vector3(0, 0, 0);
        newDragon.transform.localRotation = Quaternion.identity;
    }


    public class NftResponseData
    {
        public string image;
        public string name;
        public string description;
        public int topSpeed;
        public int acceleration;
        public string race;
        public string rarity;
    }


}

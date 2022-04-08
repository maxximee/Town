using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static EVM;
using static ImportNFTTextureExample;
using TMPro;
using UnityEngine.UI;

public class ERC721URIExample : MonoBehaviour
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

    [Header("3D Elements")]
    [SerializeField] private GameObject[] dragonPrefabs;
    [SerializeField] private GameObject dragonInitPost;

    [Header("Chain Parameters")]
    [SerializeField] private string chain = "polygon";
    [SerializeField] private string network = "mumbai"; // mainnet ropsten kovan rinkeby goerli
    [SerializeField] private string contract = "0x87C74192D5b75A85F57C67d71A84c5d41df7Da08";
    [SerializeField] private string account = "0x3d820337ed4041D4469A830B21A15FEB9C1ac9dC";
    [SerializeField] private string rpc = "https://rpc-mumbai.matic.today";
    [SerializeField] private bool testAccount = true;


    private string[] allTokenIds;
    private int currentIndex = 0;
    async void Start()
    {
        // TODO Load all tokenIds of current user. we should get the owned (where uri is current)
       if (testAccount)
        {
            PlayerPrefs.SetString("tokenIds", "0,1,2");
        }

       if (PlayerPrefs.HasKey("tokenIds")) {
            allTokenIds = PlayerPrefs.GetString("tokenIds").Split(',');
            // Load first dragon
            LoadDragon(allTokenIds[currentIndex]);
        } else
        {
            // show a purchase your first dragon message
        }
    }

    async void LoadDragon(string tokenId)
    {

        string uri = await ERC721.URI(chain, network, contract, tokenId, rpc);
        print(uri);

        // fetch json from uri
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();
        Response data = JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

        // parse json to get image uri
        string imageUri = data.image;
        print("imageUri: " + imageUri);

        print("name: " + data.name);
        name.text = data.name;

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
        dragonSprite.sprite = Sprite.Create(((DownloadHandlerTexture)textureRequest.downloadHandler).texture, new Rect(0,0,1233,1233), new Vector2(0.5f, 1f),100f);
    }

    public void LoadNextDragon()
    {

        if (currentIndex == allTokenIds.Length - 1)
        {
            currentIndex = 0;
        } else
        {
            currentIndex++;
        }
        indexText.text = (currentIndex + 1) + " of " + allTokenIds.Length;
        LoadDragon(allTokenIds[currentIndex]);
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
        LoadDragon(allTokenIds[currentIndex]);
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
        GameObject newDragon = Instantiate(dragonPrefabs[currentIndex]);
        newDragon.transform.parent = dragonInitPost.transform;
        newDragon.transform.localPosition = new Vector3(0, 0, 0);
        newDragon.transform.localRotation = Quaternion.identity;
    }
}

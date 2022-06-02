using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static LoadMarketItems;
using UnityEngine.UI;
using System.Numerics;
using static LoadPlayerNfts;

public class LoadMarketPlace : MonoBehaviour
{

    [SerializeField] private GameObject MarketItemPrefab;

    [SerializeField] private GameObject SummaryMarketItemPrefab;
    [SerializeField] private LoadMarketItems MarketCommands;

    [SerializeField] private LoadPlayerNfts NftManager;


    private string userAddress = Manager.PlayerAddress;

    private float panelXPos = -340f;

    public async void LoadUI()
    {
        foreach (MarketItem item in Manager.GetMarketItems())
        {
            GameObject panel = Instantiate(SummaryMarketItemPrefab, transform);
            panel.transform.localPosition = new UnityEngine.Vector2(panelXPos, panel.transform.localPosition.y);
            panelXPos += 180;
            Component[] allChildren = panel.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                switch (child.gameObject.name)
                {
                    case "DragonTokenId":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = "Dragon #0" + item.TokenId.ToString();
                        break;
                    case "DragonPrice":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = (Mathf.Round((float)(item.Price / Manager.TokenDecimal) * 1000f) / 1000f).ToString() + " MATIC";
                        break;
                    case "DragonSprite":
                        // TODO item.TokenId won't work once we have more tokens than unique dragon. Should be bloodType
                        string spriteNamePrefix = "DragonSD_";
                        if (item.TokenId < 10)
                        {
                            spriteNamePrefix = spriteNamePrefix + "0";
                        }
                        Sprite sp = Resources.Load<Sprite>("Dragons_2D/" + spriteNamePrefix + item.TokenId.ToString());
                        child.gameObject.GetComponent<Image>().sprite = sp;
                        break;
                    case "DragonSlot":
                        child.gameObject.SetActive(true);
                        Button ShowDetailsButton = child.gameObject.GetComponent<Button>();
                        ShowDetailsButton.onClick.AddListener(() => LoadDragonMarketItem(item));
                        break;
                    default:
                        break;

                }
            }
            Button DetailsButton = panel.gameObject.GetComponent<Button>();
            DetailsButton.onClick.AddListener(() => LoadDragonMarketItem(item));
            Image Frame = panel.gameObject.GetComponent<Image>();
            
        }
    }

    public void RefreshUI()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        panelXPos = -340f;
        LoadUI();
    }

    public async void LoadDragonMarketItem(MarketItem item)
    {
        GameObject panel = Instantiate(MarketItemPrefab, transform.parent.parent.parent);

        Dragon dragon = await NftManager.LoadDragon(item.TokenId.ToString());
        NftDragonData dragonData = await NftManager.LoadDragonData(dragon);
        Component[] allChildren = panel.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "dragonTokenId":
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = item.TokenId.ToString();
                    break;
                case "dragonRarity":
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = dragon.Rarity;
                    break;
                case "TopSpeedSlider":
                    child.gameObject.GetComponent<Slider>().value = (float)dragon.TopSpeed / 10;
                    break;
                case "TopSpeedValue":
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = ((float)dragon.TopSpeed / 10).ToString();
                    break;
                case "AccelSlider":
                    child.gameObject.GetComponent<Slider>().value = (float)dragon.Acceleration / 10;
                    break;
                case "AccelValue":
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = ((float)dragon.Acceleration / 10).ToString();
                    break;
                case "YieldSlider":
                    child.gameObject.GetComponent<Slider>().value = (float)dragon.Yield / 10;
                    break;
                case "YieldValue":
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = ((float)dragon.Yield / 10).ToString();
                    break;
                case "DietSlider":
                    child.gameObject.GetComponent<Slider>().value = (float)dragon.Diet / 10;
                    break;
                case "DietValue":
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = ((float)dragon.Diet / 10).ToString();
                    break;
                case "priceValue":
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = (Mathf.Round((float)(item.Price / Manager.TokenDecimal) * 1000f) / 1000f).ToString() + " MATIC";
                    break;
                case "sellerAddress":
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = item.Seller.Substring(0, 10) + "...";
                    break;
                case "dragonSprite":
                    string spriteNamePrefix = "DragonSD_";
                    if (dragonData._bloodType < 10)
                    {
                        spriteNamePrefix = spriteNamePrefix + "0";
                    }
                    Sprite sp = Resources.Load<Sprite>("Dragons_Sprites/" + spriteNamePrefix + dragonData._bloodType.ToString());
                    child.gameObject.GetComponent<Image>().sprite = sp;
                    break;
                case "BUY":
                    if (item.Seller.Equals(userAddress))
                    {
                        child.gameObject.SetActive(false);
                    }
                    else
                    {
                        child.gameObject.SetActive(true);
                        Button BuyButton = child.gameObject.GetComponent<Button>();
                        BuyButton.onClick.AddListener(() => MarketCommands.buyItem(item.Id, item.Price));
                    }
                    break;
                case "REMOVE":
                    if (item.Seller.Equals(userAddress))
                    {
                        child.gameObject.SetActive(true);
                        Button RemoveButton = child.gameObject.GetComponent<Button>();
                        RemoveButton.onClick.AddListener(() => MarketCommands.removeItem(item.Id));
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
                default:
                    break;

            }

        }
    }

}

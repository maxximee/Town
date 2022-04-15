using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static LoadMarketItems;
using UnityEngine.UI;
using System.Numerics;

public class LoadMarketPlace : MonoBehaviour
{

    [SerializeField] private GameObject MarketItemPrefab;
    [SerializeField] private LoadMarketItems MarketCommands;


    private string userAddress = Manager.PlayerAddress;



    private float panelXPos = -380f;


    public void LoadUI()
    {
        
        foreach (MarketItem item in Manager.GetMarketItems()) { 
            GameObject panel = Instantiate(MarketItemPrefab, transform);
            panel.transform.localPosition = new UnityEngine.Vector2(panelXPos, panel.transform.localPosition.y);
            panelXPos += 180;
            foreach (Transform child in panel.transform)
            {
                switch (child.gameObject.name)
                {
                    case "dragonTokenId":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = item.Id.ToString();
                        break;
                    case "priceValue":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = item.Price.ToString() + " MATIC";
                        break;
                    case "sellerAddress":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = item.Seller.Substring(0,6) + "...";
                        break;
                    case "BUY":
                        if (item.Seller.Equals(userAddress)) { 
                            child.gameObject.SetActive(false);
                        } else
                        {
                            child.gameObject.SetActive(true);
                            Button BuyButton = child.gameObject.GetComponent<Button>();
                            BuyButton.onClick.AddListener(() => MarketCommands.buyItem(item.Id));
                        }
                        break;
                    case "REMOVE":
                        if (item.Seller.Equals(userAddress))
                        {
                            child.gameObject.SetActive(true);
                            Button RemoveButton = child.gameObject.GetComponent<Button>();
                            RemoveButton.onClick.AddListener(() => MarketCommands.removeItem(item.Id));
                        } else
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

}

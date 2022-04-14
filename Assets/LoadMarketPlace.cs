using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static LoadMarketItems;

public class LoadMarketPlace : MonoBehaviour
{

    [SerializeField] private GameObject MarketItemPrefab;
    
    
    private string userAddress = Manager.PlayerAddress;



    private float panelXPos = -380f;


    public void LoadUI()
    {
        
        foreach (MarketItem item in Manager.GetMarketItems()) { 
            GameObject panel = Instantiate(MarketItemPrefab, transform);
            panel.transform.localPosition = new Vector2(panelXPos, panel.transform.localPosition.y);
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
                        }
                        break;
                    case "REMOVE":
                        if (item.Seller.Equals(userAddress))
                        {
                            child.gameObject.SetActive(true);
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

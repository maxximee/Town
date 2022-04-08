using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LoadMarketPlace : MonoBehaviour
{

    [SerializeField] private GameObject MarketItemPrefab;
    [SerializeField] private string userAddress = "0x3d820337ed4041D4469A830B21A15FEB9C1ac9dC";

    public CustomCallExample eventRaiseBehaviour;

    private float panelXPos = -380f;

    private void Awake()
    {
        if (eventRaiseBehaviour.marketLoadedEvent == null)
        {
            eventRaiseBehaviour.marketLoadedEvent = new UnityEvent();
        }

        // subscribe to the event
        eventRaiseBehaviour.marketLoadedEvent.AddListener(MarketLoadedEvent_Handler);
    }

    private void MarketLoadedEvent_Handler()
    {
        print("market place items loaded");
        LoadUI();
    }

    public void LoadUI()
    {
        
        foreach (MarketItem item in Manager.GetMarketItems()) { 
            GameObject panel = Instantiate(MarketItemPrefab, transform);
            panel.transform.localPosition = new Vector2(panelXPos, panel.transform.localPosition.y);
            panelXPos += 140;
            foreach (Transform child in panel.transform)
            {
                switch (child.gameObject.name)
                {
                    case "dragonName":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = "Dragon id: " + item.Id.ToString();
                        break;
                    case "priceValue":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = item.Price.ToString() + " MATIC";
                        break;
                    case "BuyLabel":
                        if (item.SellerAddress.Equals(userAddress))
                        {
                            child.gameObject.GetComponent<TextMeshProUGUI>().text = "REMOVE";
                        }
                        else
                        {
                            child.gameObject.GetComponent<TextMeshProUGUI>().text = "BUY";
                        }
                        break;
                    case "sellerAddress":
                        child.gameObject.GetComponent<TextMeshProUGUI>().text = item.SellerAddress.Substring(0,6) + "...";
                        break;
                    default:
                        break;

                }

            }
        }
    }

}

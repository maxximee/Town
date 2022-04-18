using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;

public class listForSaleButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField PriceInputField;
    [SerializeField] private TextMeshProUGUI TokenIdText;
    public LoadMarketItems marketFunctions;

    public void ListForSale() {
        Debug.Log("token: " + TokenIdText.text + " price " + PriceInputField.text);
        BigInteger tokenIdAsInt = BigInteger.Parse(TokenIdText.text);
        BigInteger priceAsInt = BigInteger.Parse(PriceInputField.text);
        marketFunctions.listItemForSale(tokenIdAsInt, priceAsInt);
    }
}

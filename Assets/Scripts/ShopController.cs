using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] smallBoxPriceTxt;
    private float smallBoxpPrice;

    public void SetPriceSB(float price)
    {
        foreach (var item in smallBoxPriceTxt)
        {
            item.text = price.ToString();
        }
        smallBoxpPrice = price;
        
    }
}

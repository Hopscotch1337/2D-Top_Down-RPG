using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singelton<EconomyManager>
{
    private TMP_Text goldCoinsText;
    const string COIN_AMOUNT_TEXT = "GoldAmountText";
    private int currentGoldAmount = 0;
    

 

    protected override void Awake()
    {
        base.Awake();

    }



    public void UpdateGoldCoins(int value)
    {   
        if (goldCoinsText == null)
        {
            goldCoinsText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        currentGoldAmount += value;
        goldCoinsText.text = currentGoldAmount.ToString("D4"); // Format as 4 digits with leading zeros
    }

}

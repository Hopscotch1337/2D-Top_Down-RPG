using UnityEngine;
using TMPro;


public class EconomyManager : Singelton<EconomyManager>
{
    private TMP_Text goldCoinsText;
    const string COIN_AMOUNT_TEXT = "GoldAmountText";
    public int CurrentGoldAmount; //{ get; private set; } = 0;
private void Start() {
        UpdateGoldCoins(0);
}
    public void UpdateGoldCoins(int value)
    {   
        if (goldCoinsText == null)
        {
            goldCoinsText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        CurrentGoldAmount += value;
        goldCoinsText.text = CurrentGoldAmount.ToString("D4"); // Format as 4 digits with leading zeros
    }

}

using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateMoneyText();
    }

    void Update()
    {
        // Only updates when value changes
        if (moneyText.text != PlayerMoney.money.ToString())
            UpdateMoneyText();
    }

    void UpdateMoneyText()
    {
        moneyText.text = "$ " + PlayerMoney.money.ToString();
    }
}

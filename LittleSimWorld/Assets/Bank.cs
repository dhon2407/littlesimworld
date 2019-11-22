using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CharacterStats;
[System.Serializable]
public class Bank : MonoBehaviour
{
    public TMP_InputField InputField;
    public TextMeshProUGUI BankMoneyNumber;
    public double MoneyInBank;
    public float PercentagePerDay = 0.01f;
    public static Bank Instance;
    public GameObject CanvasGO;
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
    void Update()
    {
        if (CanvasGO.activeSelf && Vector2.Distance(GameLibOfMethods.player.transform.position, transform.position) > 1.5f)
        {
            HideBankUI();
        }
    }
    public void Deposit()
    {
        int inputInt = int.Parse(InputField.text);
        if (inputInt > PlayerStatsManager.Instance.Money)
        {
            GameLibOfMethods.CreateFloatingText("Not enough money!", 3);
            return;
        }
        PlayerStatsManager.Instance.Money -= inputInt;
        MoneyInBank += inputInt;
        UpdateBalance();

    }
    public void Withdraw()
    {
        PlayerStatsManager.Instance.Money += (float)MoneyInBank;
        MoneyInBank = 0;
        UpdateBalance();

    }
    public void AddPercentageToBalance()
    {
        MoneyInBank += MoneyInBank * PercentagePerDay;
        UpdateBalance();
    }

    public void UpdateBalance()
    {
        BankMoneyNumber.text =  System.Math.Round(MoneyInBank,2).ToString();
    }

    public void ShowBankUI()
    {
        CanvasGO.SetActive(true);
        StatsTooltip.Instance.HideDescription();

    }
    public void HideBankUI()
    {
        CanvasGO.SetActive(false);
        StatsTooltip.Instance.HideDescription();
    }
    public void SwitchBankUI()
    {
        CanvasGO.SetActive(!CanvasGO.activeSelf);
        StatsTooltip.Instance.HideDescription();
    }

}

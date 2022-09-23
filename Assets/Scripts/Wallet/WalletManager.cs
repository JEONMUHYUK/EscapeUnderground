using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletManager : SingleTon<WalletManager>
{
    [SerializeField] static float moneyBet;
    [SerializeField] ToggleGroup setBetMoneyTogglesGroup = null;
    [SerializeField] Toggle[] betMoneyToggles = null;
    [SerializeField] MyWallet myWallet = null;

    private void Awake()
    {
        // MyWallet�� �ı����� �ʴ´�.
        // MyWallet�� �ϳ� �̻��̸� �ı��ϰ� �ϳ��� �����.
        WalletManager[] obj = FindObjectsOfType<WalletManager>();
        if (obj.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);


    }

    // ���� ������ �� Ȱ��ȭ�� ���ñ� ���� ��۷� cost�� �����Ѵ�.
    public float SetCost()
    {
        
        setBetMoneyTogglesGroup = FindObjectOfType<ToggleGroup>();
        betMoneyToggles = setBetMoneyTogglesGroup.GetComponentsInChildren<Toggle>();
        Toggle activeBetToggle = Array.Find(betMoneyToggles, delegate (Toggle toggle) { return toggle.isOn == true; });
        float cost = float.Parse(activeBetToggle.name);
        return cost;
    }

    // ���� ���� �Ѵ�.
    public void BetMoney(float cost)
    {
        myWallet.MyMoney = -cost;
        moneyBet += cost;
        myWallet.moneyUpdate();
    }

    // ���� ���� �޴´�.
    public void GiveBackMoney()
    {
        myWallet.MyMoney = moneyBet;
        moneyBet = 0;
        myWallet.moneyUpdate();
    }

    // �¸��� ���� ������.
    public void GetMoney()
    {
        myWallet.MyMoney = moneyBet * 2;
        moneyBet = 0;
        myWallet.moneyUpdate();
    }

    // �й�� ���� �Ҵ´�.
    public void LoseMoney() => moneyBet = 0;
}

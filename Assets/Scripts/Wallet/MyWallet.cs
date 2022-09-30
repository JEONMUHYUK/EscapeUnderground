using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyWallet : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI zeraCoinTxt = null;
    [SerializeField] TextMeshProUGUI aceCoinTxt = null;
    [SerializeField] TextMeshProUGUI dappXCoinTxt = null;

    [SerializeField] DappxAPIDataConroller dappxAPIDataConroller = null;

    private void Awake()
    {
        // MyWallet�� �ı����� �ʴ´�.
        // MyWallet�� �ϳ� �̻��̸� �ı��ϰ� �ϳ��� �����.

        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            zeraCoinTxt = GameObject.FindGameObjectWithTag("Wallet").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            aceCoinTxt = GameObject.FindGameObjectWithTag("Wallet").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            dappXCoinTxt = GameObject.FindGameObjectWithTag("Wallet").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        }
        dappxAPIDataConroller = FindObjectOfType<DappxAPIDataConroller>();
    }


    public void MoneyUpdate()
    {
        
        zeraCoinTxt.text = "ZeraCoin : " + dappxAPIDataConroller.ZeraBalanceInfo.data.balance;
        aceCoinTxt.text = "AceCoin : " + dappxAPIDataConroller.AceBalanceInfo.data.balance;
        dappXCoinTxt.text = "DappXCoin : " + dappxAPIDataConroller.DappXBalanceInfo.data.balance;
    }


}

using Newtonsoft.Json.Linq;
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
        // MyWallet은 파괴되지 않는다.
        // MyWallet이 하나 이상이면 파괴하고 하나만 남긴다.

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
        StartCoroutine(BringMoney());
        StartCoroutine(aBringMoney());
        StartCoroutine(dBringMoney());
    }

    IEnumerator BringMoney()
    {

            yield return dappxAPIDataConroller.ZeraBalanceInfo.data.balance > 0;
            zeraCoinTxt.text = "ZeraCoin : " + dappxAPIDataConroller.ZeraBalanceInfo.data.balance;


    }
    IEnumerator aBringMoney()
    {

            yield return dappxAPIDataConroller.AceBalanceInfo.data.balance > 0;
            aceCoinTxt.text = "AceCoin : " + dappxAPIDataConroller.AceBalanceInfo.data.balance;


    }
    IEnumerator dBringMoney()
    {

            yield return dappxAPIDataConroller.DappXBalanceInfo.data.balance > 0;
            dappXCoinTxt.text = "DappXCoin : " + dappxAPIDataConroller.DappXBalanceInfo.data.balance;

    }


}

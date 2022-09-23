using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyWallet : MonoBehaviour
{
    [SerializeField] static float money;
    [SerializeField] TextMeshProUGUI walletTxt = null;

    public Action moneyUpdate;
    public float MyMoney { get { return money; } set { money += value; } }
    private void Awake()
    {
        // MyWallet�� �ı����� �ʴ´�.
        // MyWallet�� �ϳ� �̻��̸� �ı��ϰ� �ϳ��� �����.
        MyWallet[] obj = FindObjectsOfType<MyWallet>();
        if (obj.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
        moneyUpdate = delegate () { walletTxt.text = money.ToString(); };
    }

    private void Start()
    {
        money = 10000;
        moneyUpdate();
    }

    private void Update()
    {
        if (walletTxt == null) walletTxt = GameObject.FindGameObjectWithTag("Wallet").GetComponent<TextMeshProUGUI>();
        
    }

}

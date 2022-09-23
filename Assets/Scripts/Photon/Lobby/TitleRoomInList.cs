using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleRoomInList : MonoBehaviourPun
{
    public string roomName          { get; set; }           // �� �̸� ������Ƽ
    public string roomInPlayer      { get; set; }           // �� �÷��̾� �� ������Ƽ
    public string maxRoomInPlayer   { get; set; }           // �ִ� �÷��̾� ��
    public float cost   { get; set; }                       // ���� cost

    public TextMeshProUGUI roomTitleTxt         = null;     // �� Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI RoomInPlayerCntText  = null;     // �÷��̾� �� �ؽ�Ʈ
    public TextMeshProUGUI costText             = null;     // �÷��̾� �� �ؽ�Ʈ

    MyWallet myWallet = null;   // myWallet
    private void OnEnable()
    {
        myWallet = FindObjectOfType<MyWallet>();
    }
    private void Update()
    {
        costText.text = "Cost : " + cost.ToString();
        roomTitleTxt.text = roomName;
        RoomInPlayerCntText.text = roomInPlayer + " / " + maxRoomInPlayer;
    }
    public void OnClickJoinRoom()
    {
        // Cost�� ������ �� ���ٸ� ����.
        if (myWallet.MyMoney < cost) return;

        // �濡 ���ٸ� �ڽ�Ʈ�� �����ϰ� ���� �Ѵ�.
        //WalletManager.Instance.BetMoney(cost);
        PhotonNetwork.JoinRoom(roomName);
    }

}

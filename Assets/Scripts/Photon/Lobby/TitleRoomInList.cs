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

    public TextMeshProUGUI roomTitleTxt         = null;     // �� Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI RoomInPlayerCntText  = null;     // �÷��̾� �� �ؽ�Ʈ
    public TextMeshProUGUI costText             = null;     // �÷��̾� �� �ؽ�Ʈ

    AudioManager audioManager   = null;   // audioManager
    DappxAPIDataConroller dappxAPIDataConroller = null;
    private void OnEnable()
    {
        audioManager            = FindObjectOfType<AudioManager>();
        dappxAPIDataConroller   = FindObjectOfType<DappxAPIDataConroller>();
    }
    private void Update()
    {
        if (roomTitleTxt.text == "RoomTitle") roomTitleTxt.text = roomName;
        RoomInPlayerCntText.text = roomInPlayer + " / " + maxRoomInPlayer;
    }
    public void OnClickJoinRoom()
    {
        // Cost�� ������ �� ���ٸ� ����.
        if (dappxAPIDataConroller.ZeraBalanceInfo.data.balance < dappxAPIDataConroller.BetSettings.data.bets[0].amount) return;

        audioManager.SoundPlay(audioManager.ClickSound);
        // �濡 ���ٸ� �ڽ�Ʈ�� �����ϰ� ���� �Ѵ�.
        PhotonNetwork.JoinRoom(roomName);
    }

}

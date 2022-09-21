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

    private void Update()
    {
        roomTitleTxt.text = roomName;
        RoomInPlayerCntText.text = roomInPlayer + " / " + maxRoomInPlayer;
    }
    public void OnClickJoinRoom() => PhotonNetwork.JoinRoom(roomName);

}

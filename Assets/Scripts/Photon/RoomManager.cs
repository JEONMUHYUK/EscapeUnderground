using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [Header("( Texts )")]
    [SerializeField] private TextMeshProUGUI masterClientNameText = null;          // ������ Ŭ���̾�Ʈ NameText
    [SerializeField] private TextMeshProUGUI challengerClientNameText = null;      // ç���� Ŭ���̾�Ʈ NameText
    [SerializeField] private TextMeshProUGUI cntPlayersTxt = null;                 // �÷��̾� �� Text
    [SerializeField] private TextMeshProUGUI roomTitleTxt = null;                  // �� ���� Text


    // ���� �Ŵ���
    AudioManager audioManager = null;
    private void OnEnable()
    {
        audioManager = FindObjectOfType<AudioManager>();
        roomTitleTxt.text = PhotonNetwork.CurrentRoom.Name;



    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        cntPlayersTxt.text = $"[ {PhotonNetwork.CurrentRoom.Players.Count} / {PhotonNetwork.CurrentRoom.MaxPlayers} ]";
        SetRoomInfo();

        // �θ� �̻� ���̸� �ٷ� ����.
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            OnClickLoadGameScene();
        }
    }


    // ���Ӿ����� �̵��Ѵ�.
    void OnClickLoadGameScene()
    {
        // ������ �����ؾ� ������ �ս��� ���� �ʱ� �����̴�.
        // ���� ������ Ŭ���̾�Ʈ�� �ƴҽ� ���� ������ �ص� ������ ����ȭ ���� �ʴ´�.
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) return;
        PhotonNetwork.LoadLevel("GameScene");
    }

    // �������� ������Ʈ �Ѵ�.
    void SetRoomInfo()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                masterClientNameText.text = "MasterClient : " + player.NickName.ToString();
                return;
            }
            if (player.IsMasterClient) masterClientNameText.text = "MasterClient : " + player.NickName.ToString();
            else challengerClientNameText.text = "ChallengerClient : " + player.NickName.ToString();
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [Header("( Texts )")]
    [SerializeField] private TextMeshProUGUI masterClientNameText;          // ������ Ŭ���̾�Ʈ NameText
    [SerializeField] private TextMeshProUGUI challengerClientNameText;      // ç���� Ŭ���̾�Ʈ NameText
    [SerializeField] private TextMeshProUGUI cntPlayersTxt;                 // �÷��̾� �� Text
    [SerializeField] private TextMeshProUGUI roomTitleTxt;                  // �� ���� Text

    [Header("( Buttons )")]
    [SerializeField] private Button leaveRoomBtn = null;
    [SerializeField] private Button gameStartBtn = null;

    
    private void OnEnable()
    {
        roomTitleTxt.text = PhotonNetwork.CurrentRoom.Name;
        leaveRoomBtn.onClick.AddListener(delegate { onClickLeaveRoom(); });
        gameStartBtn.onClick.AddListener(delegate { OnClickLoadGameScene(); });
    }

    private void Update()
    {
        cntPlayersTxt.text = $"[ {PhotonNetwork.CurrentRoom.Players.Count} / {PhotonNetwork.CurrentRoom.MaxPlayers} ]";
        SetRoomInfo();
    }

    // ���� ������.
    void onClickLeaveRoom()
    {
        WalletManager.Instance.GiveBackMoney();
        PhotonNetwork.LeaveRoom();
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
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].IsMasterClient)
                masterClientNameText.text = "MasterClient : " + PhotonNetwork.PlayerList[i].NickName.ToString() ;

            else
                challengerClientNameText.text = "ChallengerClient : " +  PhotonNetwork.PlayerList[i].NickName.ToString();
        }
    }
}

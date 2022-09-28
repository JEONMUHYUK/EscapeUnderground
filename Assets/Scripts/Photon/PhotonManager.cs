using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [Header("[ Panels ]")]
    [SerializeField] private LobbyManager       lobbyPanel                  = null;     // �κ� �г�
    [SerializeField] private RoomManager        roomPanel                   = null;     // �� �г�
    [SerializeField] private GridLayoutGroup    roomListPanel               = null;     // �븮��Ʈ �г�

    [Header("[ Components ]")]
    [SerializeField] UIChatManager       chatManager             = null;                     // ä�� �Ŵ���
    [SerializeField] AudioManager        audioManager            = null;                     // ���� �Ŵ���
    [SerializeField] JsonDataController  jsonDataController      = null;                     // Json Data
    [SerializeField] MyWallet            myWallet                = null;                     // ����

    [Header("[ Lists ]")]
    [SerializeField] List<RoomInfo>      uiRoomList              = new List<RoomInfo>();     // �븮��Ʈ UI ���� ����Ʈ.


    private void Awake()
    {
        // ���� ���۽� ��ũ�� ����� ������ 16 : 9 ������ ������ ���ڰ��� ��üȭ�� ����
        Screen.SetResolution(1920, 1080, false);

        //AutomaticallySyncScene �� �濡 �ִ� ��� Ŭ���̾�Ʈ���� �ڵ������� ������ Ŭ���̾�Ʈ�� ������ ������ �ε��Ų��.
        PhotonNetwork.AutomaticallySyncScene = true;

        
        chatManager         = FindObjectOfType<UIChatManager>();        // UIChatManager
        audioManager        = FindObjectOfType<AudioManager>();         // audioManager
        myWallet            = FindObjectOfType<MyWallet>();             // myWallet
        jsonDataController  = FindObjectOfType<JsonDataController>();   //Json ����Ÿ
    }
    void Start()
    {
        OnClickConnectToMasterServer(); // ������ ���� ����

        // �г��� ����
        PhotonNetwork.LocalPlayer.NickName = jsonDataController.PlayerName;
    }

    // ������ ���� ���� ��û.
    public void OnClickConnectToMasterServer() => PhotonNetwork.ConnectUsingSettings();

    // ������ ���� ���� ����.
    public void OnClickDiconnectToMasterServer() => PhotonNetwork.Disconnect();

    // �κ� �����ϱ�.
    public void OnClickJoinLobby()
    {
        PhotonNetwork.JoinLobby();      // �κ񿬰�
        chatManager.ConnectedMyChat();  // ê ����
    }


    // Photon Cloud Server�� ���ӿ� ������ �Ҹ��� �ݹ� �Լ�.
    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();

        // ���� �ؽ�Ʈ ������Ʈ
        myWallet.moneyUpdate();
    }

    // ������ ���� ������ ������ �� ȣ��Ǵ� �Լ�.
    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("OnDisConnected : " + cause);
  
    #region Lobby
    // �κ� ���ӽ� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        chatManager.ClearText();
        lobbyPanel.gameObject.SetActive(true);
    } 

    // �κ� ������ ȣ�� �Ǵ� �ݹ� �Լ�
    public override void OnLeftLobby() => lobbyPanel.gameObject.SetActive(false);
    #endregion

    #region Room
    // �濡 ���ӽ� ȣ��Ǵ� �ݹ� �Լ�.
    public override void OnJoinedRoom()
    {
        lobbyPanel.gameObject.SetActive(false);
        roomPanel.gameObject.SetActive(true);
        WalletManager.Instance.BetMoney((float)PhotonNetwork.CurrentRoom.CustomProperties["Cost"]);


    }
    // �÷��̾ �濡 ����� ���� ������Ʈ
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomPanel.CntPlayersTxt = string.Format("[ {0} / {1} ]", PhotonNetwork.CurrentRoom.Players.Count, PhotonNetwork.CurrentRoom.MaxPlayers);
        roomPanel.ChallengerClientNameText = "Challenger : " +  newPlayer.NickName;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel("GameScene");
    }

    // ���������� �������� ���
    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("OnJoinRandomFailed : " + message);

    // Room�� ������ ȣ�� �Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        if (!PhotonNetwork.InLobby) return;
        roomPanel.gameObject.SetActive(false);
    }
    #endregion

    #region RoomListUI
    // ��� ������Ʈ�� �޾ƾ� �ϱ� ������ ��� Ȱ��ȭ �Ǿ��ִ� �Ŵ������� üũ�Ѵ�.
    // �������� �� ����Ʈ�� �޾ƿ��� �ݹ� �Լ��̴�.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ������Ʈ �ñ� : �κ�� ���ӽñ�, �ٸ� Ŭ���̾�Ʈ�� ���� ���� ��, ���� �ϳ� �������.
        // Room Tag�� ���� ������Ʈ�� ã�Ƽ� ȸ���Ѵ�. 
        // ȸ���� ���ϸ� ���� ������ ����ؼ� �����ְ� �ȴ�.

        // UIRoom�� ������Ʈ �Ǵ� �ñ⸶�� room������ ��ü ȸ��
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
            RoomsPool.Instance.Release(obj);


        // �� ������Ʈ �ݹ� �Լ� ������ �������� UIroomList ������Ʈ 
        foreach (RoomInfo roomInfo in roomList)
        {
            // ������ ���� ���ſ��� ���������� Ȯ���Ͽ� UIroomList���� ����
            // �ƴ϶�� �뿡 ���ԵǾ��ִ��� Ȯ���Ͽ� ����.
            if (roomInfo.RemovedFromList) uiRoomList.Remove(roomInfo);
            else if (!uiRoomList.Contains(roomInfo)) uiRoomList.Add(roomInfo);
        }

        // UIroomList �� ��ŭ �������� ���� �����Ѵ�.
        foreach (RoomInfo roomInfo in uiRoomList) CreateRoomUI(roomInfo);

    }

    // UI ������ �߰�
    void CreateRoomUI(RoomInfo roomInfo)
    {
        TitleRoomInList roomObj = RoomsPool.Instance.Get(transform.position).GetComponent<TitleRoomInList>();
        roomObj.transform.SetParent(roomListPanel.transform);
        roomObj.roomName = roomInfo.Name;
        roomObj.roomInPlayer = roomInfo.PlayerCount.ToString();
        roomObj.maxRoomInPlayer = roomInfo.MaxPlayers.ToString();

        // Ŀ���� ������Ƽ�� ���� ���� �߰��Ѵ�.
        //roomInfo.CustomProperties.Add("cost", WalletManager.Instance.SetCost());
        roomObj.cost = (float)roomInfo.CustomProperties["Cost"];
    }

    #endregion

    // Ÿ��Ʋ �� ���� �ؽ�Ʈ ���.
    private void OnGUI()
    {
        GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
        gUIStyle.fontSize = 20;
        gUIStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(0f, 0f, 350f, 50f), "ServeState : " + PhotonNetwork.Server.ToString(), gUIStyle);
    }

    #region ContextMenu
    // �ν����ͻ��� �ɼ� �߰�.
    [ContextMenu("[Info]")]
    public void RoomInfo()
    {
        Debug.Log("ClickInfo");
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("���� �� �̸� : " + PhotonNetwork.CurrentRoom);
            Debug.Log("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("���� �� �ִ� �ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);
            Debug.Log("�κ� �ִ� ��? : " + PhotonNetwork.InLobby);
            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) 
                playerStr += PhotonNetwork.PlayerList[i].NickName + ",";
            Debug.Log(playerStr);

            
        }
        else
        {
            Debug.Log("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            // �뿡 �������� ���� �÷��̾��� ���� �����´�.
            Debug.Log("�� ���� : " + PhotonNetwork.CountOfRooms);
            // �� �ȿ� �ִ� �÷��̾��� ���� �����´�.
            Debug.Log("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            // ����Ǿ� �ִ� �÷��̾��� �� ���� �����´�.
            Debug.Log("�κ� �ִ� ��? : " + PhotonNetwork.InLobby);
            Debug.Log("���� �Ǿ��� �� ? : " + PhotonNetwork.IsConnected);
        }
    }
    #endregion
}

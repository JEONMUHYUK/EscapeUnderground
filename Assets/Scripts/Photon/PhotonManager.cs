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
    [Header("[ Texts ]")]
    [SerializeField] private TextMeshProUGUI    ServerStateTxt              = null;     // ���� ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI    clentStateTxt               = null;     // Ŭ���̾�Ʈ ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI    clentNickNameTxt            = null;     // Ŭ���̾�Ʈ �г��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI    countOfRoomsTxt             = null;     // �� ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI    countOfPlayersOnMasterTxt   = null;     // ������ ���� �� �ִ� �÷��̾� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI    countOfPlayersInRoomsTxt    = null;     // ���� ���� �� �ִ� �÷��̾� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI    countOfPlayersTxt           = null;     // �÷��̾� �� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI    lobbyStateTxt               = null;     // �κ� ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI    roomStateTxt                = null;     // �� ���� �ؽ�Ʈ


    [Header("[ Panels ]")]
    [SerializeField] private LobbyManager       lobbyPanel                  = null;     // �κ� �г�
    [SerializeField] private RoomManager        roomPanel                   = null;     // �� �г�
    [SerializeField] private GridLayoutGroup    roomListPanel               = null;     // �븮��Ʈ �г�

    [Header("[ Buttons ]")]
    [SerializeField] private Button connectServerBtn        = null;     // ���� ���� ��ư
    [SerializeField] private Button disConnectServerBtn     = null;     // ���� ���� ���� ��ư
    [SerializeField] private Button joinLobbyBtn            = null;     // �κ� ���� ��ư

    // ä�� �Ŵ���
    UIChatManager chatManager = null;

    // �븮��Ʈ UI ���� ����Ʈ.
    List<RoomInfo> uiRoomList = new List<RoomInfo>();

    // ���� �Ŵ���
    AudioManager audioManager = null;

    JsonDataController jsonDataController = null;

    // ����
    MyWallet myWallet = null;
    private void Awake()
    {
        // ���� ���۽� ��ũ�� ����� ������ 16 : 9 ������ ������ ���ڰ��� ��üȭ�� ����
        Screen.SetResolution(1920, 1080, false);

        //AutomaticallySyncScene �� �濡 �ִ� ��� Ŭ���̾�Ʈ���� �ڵ������� ������ Ŭ���̾�Ʈ�� ������ ������ �ε��Ų��.
        PhotonNetwork.AutomaticallySyncScene = true;

        // UIChatManager
        chatManager = FindObjectOfType<UIChatManager>();

        audioManager = FindObjectOfType<AudioManager>();

        myWallet = FindObjectOfType<MyWallet>();

        //Json ����Ÿ
        jsonDataController = FindObjectOfType<JsonDataController>();
    }
    void Start()
    {
        
        OnClickConnectToMasterServer();
        // �г��� ����
        PhotonNetwork.LocalPlayer.NickName = jsonDataController.PlayerName;

        ServerStateTxt.text = "ServerState : DisConnected";
        clentNickNameTxt.text = "Client NickName : None";

        connectServerBtn.onClick.AddListener(delegate { OnClickConnectToMasterServer(); audioManager.SoundPlay(audioManager.ClickSound); });
        disConnectServerBtn.onClick.AddListener(delegate { OnClickDiconnectToMasterServer(); audioManager.SoundPlay(audioManager.ClickSound); });
        joinLobbyBtn.onClick.AddListener(delegate { OnClickJoinLobby(); audioManager.SoundPlay(audioManager.ClickSound); });
    }

    void Update()
    {

        // ���� ���� ������Ʈ
        ServerStateTxt.text = "ServerState : " + PhotonNetwork.Server;
        // Ŭ���̾�Ʈ�� ���� ���¸� �����´�.
        clentStateTxt.text = "Clent_State : " + PhotonNetwork.NetworkClientState.ToString();
        // ���̺� ���� ������ �����´�.
        countOfRoomsTxt.text = "CountOfRooms : " + PhotonNetwork.CountOfRooms.ToString();
        // �뿡 �������� ���� �÷��̾��� ���� �����´�.
        countOfPlayersOnMasterTxt.text = "CountOfPlayersOnMaster : " + PhotonNetwork.CountOfPlayersOnMaster.ToString();
        // �� �ȿ� �ִ� �÷��̾��� ���� �����´�.
        countOfPlayersInRoomsTxt.text = "CountOfPlayersInRooms : " + PhotonNetwork.CountOfPlayersInRooms.ToString();
        // ����Ǿ� �ִ� �÷��̾��� �� ���� �����´�.
        countOfPlayersTxt.text = "CountOfPlayers : " + PhotonNetwork.CountOfPlayers.ToString();
        // �� ���� ������Ʈ
        roomStateTxt.text = "Room_State : " + PhotonNetwork.InRoom;
        // �κ� ���� ������Ʈ
        lobbyStateTxt.text = "Lobby_State : " + PhotonNetwork.InLobby;
    }

    // ������ ���� ���� ��û.
    public void OnClickConnectToMasterServer() => PhotonNetwork.ConnectUsingSettings();

    // ������ ���� ���� ����.
    public void OnClickDiconnectToMasterServer() => PhotonNetwork.Disconnect();

    // �κ� �����ϱ�.
    public void OnClickJoinLobby()
    {
        PhotonNetwork.JoinLobby();     
        // ê ����
        chatManager.ConnectedMyChat();
    }


    // Photon Cloud Server�� ���ӿ� ������ �Ҹ��� �ݹ� �Լ�.
    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
        ServerStateTxt.text = "ServerState : Sucess Connected Master Server";
        // ���� �ؽ�Ʈ ������Ʈ
        myWallet.moneyUpdate();
    }

    // ������ ���� ������ ������ �� ȣ��Ǵ� �Լ�.
    public override void OnDisconnected(DisconnectCause cause) =>
        ServerStateTxt.text = "ServerState : DisConnected Master Server  ->" + cause;

  
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
    // ���������� �������� ���
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        return;
    }
    // Room�� ������ ȣ�� �Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        if (!PhotonNetwork.InLobby) return;
        roomPanel.gameObject.SetActive(false);
        roomStateTxt.text = "Room_State : Left Room";
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


}

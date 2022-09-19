using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [Header("[ Texts ]")]
    [SerializeField] private TextMeshProUGUI ServerStateTxt = null;
    [SerializeField] private TextMeshProUGUI clentStateTxt = null;
    [SerializeField] private TextMeshProUGUI clentNickNameTxt = null;
    [SerializeField] private TextMeshProUGUI countOfRoomsTxt = null;
    [SerializeField] private TextMeshProUGUI countOfPlayersOnMasterTxt = null;
    [SerializeField] private TextMeshProUGUI countOfPlayersInRoomsTxt = null;
    [SerializeField] private TextMeshProUGUI countOfPlayersTxt = null;
    [SerializeField] private TextMeshProUGUI lobbyStateTxt = null;
    [SerializeField] private TextMeshProUGUI roomStateTxt = null;


    [Header("[ InputFields ]")]
    [SerializeField] private TMP_InputField roomInput = null;
    [SerializeField] private TMP_InputField nickNameInput = null;

    [Header("[ Panels ]")]
    [SerializeField] private LobbyManager lobbyPanel = null;
    [SerializeField] private RoomManager roomPanel = null;
    [SerializeField] private GridLayoutGroup roomListPanel;

    void Start()
    {


        ServerStateTxt.text = "ServerState : DisConnected";
        clentNickNameTxt.text = "Client NickName : None";

    }

    void Update()
    {


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


        //lobbyStateTxt.text = PhotonNetwork.InLobby ? "Lobby_State : In Lobby" : "Lobby_State : Not Lobby";
        //roomStateTxt.text = PhotonNetwork.InRoom ? "Room_State : In Room" : "Room_State : Not Room";

    }




    #region Photon_Method

    // ������ ���� ���� ��û.
    public void OnClickConnectToMasterServer() => PhotonNetwork.ConnectUsingSettings();

    // ������ ���� ���� ����.
    public void OnClickDiconnectToMasterServer() => PhotonNetwork.Disconnect();

    // �κ� �����ϱ�.
    public void OnClickJoinLobby()
    {
        if (nickNameInput.text.Length == 0) return;
        PhotonNetwork.JoinLobby();
    }

    // ���� ������ Join ������ �� ����
    public void OnClickJoinOnCreateRoom()
    {
        // �� �̸��� ������ �ʾҴٸ�
        if (roomInput.text.Length == 0)
        {
            Debug.Log("###### Input Room Name ");
            return;
        }
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null); // �κ� �����ϱ�.

    }

    // ���� ������.
    public void OnClickLeaveRoom()
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("###### Not In Room.");
            return;
        }
        PhotonNetwork.LeaveRoom();
    }

    // �κ� ������.
    public void OnClickLeaveLobby()
    {
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("###### Not In Lobby.");
            return;
        }

        PhotonNetwork.LeaveLobby();
    }

    // ���� �����Ϸ���, Connect �Ǿ��ְų� Lobby�� �������־�� �Ѵ�.

    public void OnClickCreateRoom()
    {
        // �� �����ϰ�, ����.
        // �� �̸�, �ִ� �÷��̾� ��, ����� ���� ���� ����.
        // �� CreateRoom�� ������ ���̸��� �־����� JoinOnCreate�� �־����� �ʴ���?
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });
    }



    public void SetNickName(string nickName)
    {
        if (!PhotonNetwork.IsConnected)
        {
            clentNickNameTxt.text = "Client_NickName : Disconnected";
            return;
        }
        // Ŭ���̾�Ʈ�� �г����� �����Ѵ�.
        // LoadBalancingClient Ŭ���� �� �ִ� ���� ������ nickname�� �������Ѵ�.
        PhotonNetwork.LocalPlayer.NickName = nickName;
        clentNickNameTxt.text = "Client_NickName : " + PhotonNetwork.LocalPlayer.NickName;
    }
    #endregion




    #region Photon_CallBack_Functions

    /// <summary>
    /// Photon Cloud Server�� ���ӿ� ������ �Ҹ��� �ݹ� �Լ�.
    /// PhotonNetwork.ConnectUsingSettings()�� �����ϸ� �Ҹ���.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        ServerStateTxt.text = "ServerState : Sucess Connected Master Server";

    }
    // ������ ���� ������ ������ �� ȣ��Ǵ� �Լ�.
    public override void OnDisconnected(DisconnectCause cause)
    {
        
        ServerStateTxt.text = "ServerState : DisConnected Master Server " + cause;
    }

    // �κ� ���ӽ� ȣ��Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        lobbyStateTxt.text = "Lobby_State : Joined Lobby";
        lobbyPanel.gameObject.SetActive(true);

        //Debug.Log("#########RoomCnt : " + GameObject.FindGameObjectsWithTag("Room").Length);
        //foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
        //{
        //    RoomsPool.Instance.Release(obj);
        //}
    }

    // �濡 ���ӽ� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        lobbyPanel.gameObject.SetActive(false);
        roomPanel.gameObject.SetActive(true);
        roomStateTxt.text = "Room_State : Joined Room";
    }



    // �κ� ������ ȣ�� �Ǵ� �Լ�
    public override void OnLeftLobby()
    {
        lobbyPanel.gameObject.SetActive(false);
        lobbyStateTxt.text = "Lobby_State : Left Lobby";
    }

    // �κ� ������ ȣ�� �Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        roomPanel.gameObject.SetActive(false);
        //lobbyPanel.gameObject.SetActive(true);
       
        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("Not In Lobby");
        }
        StartCoroutine(ConnetcedLobby());
        roomStateTxt.text = "Room_State : Left Room";


    }

    // �뿡�� ������ �����ͼ����� ���ӵ��ڸ��� �κ�� ���� ��û�� ���� �ڷ�ƾ
    IEnumerator ConnetcedLobby()
    {
        while (true)
        {
            yield return null;
            if (PhotonNetwork.NetworkClientState.ToString() == "ConnectedToMasterServer")
            {
                PhotonNetwork.JoinLobby();
                yield break;
            }
        }

    }

    // ��� ������Ʈ�� �޾ƾ� �ϱ� ������ ��� Ȱ��ȭ �Ǿ��ִ� �Ŵ������� üũ�Ѵ�.
    // �������� �� ����Ʈ�� �޾ƿ��� �ݹ� �Լ��̴�.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Room Tag�� ���� ������Ʈ�� ã�Ƽ� ȸ���Ѵ�. 
        // ȸ���� ���ϸ� ���� ������ ����ؼ� �����ְ� �ȴ�.
        Debug.Log("RoomListCnt : " + roomList.Count);
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
        {
            RoomsPool.Instance.Release(obj);
        }
        // �� ������ ���� 
        foreach (RoomInfo roomInfo in roomList)
        {
            //RoomInfo room = roomList[i];
            TitleRoomInList roomObj = RoomsPool.Instance.Get(transform.position).GetComponent<TitleRoomInList>();
            Debug.Log("Clone room : " + roomObj.name);
            roomObj.transform.SetParent(roomListPanel.transform);
            roomObj.roomName = roomInfo.Name;
            roomObj.roomInPlayer = roomInfo.PlayerCount.ToString();
            roomObj.maxRoomInPlayer = roomInfo.MaxPlayers.ToString();
        }
    }





    // ��ȿ��� �κ��� �븮��Ʈ ������ ��� ���� ���̷��� �ڵ�
    public void PullRoomList()
    {
        BringRoom roomPoller = gameObject.AddComponent<BringRoom>();
        roomPoller.OnGetRoomsInfo
        (
            (roomInfos) =>
            {
                // �븮��Ʈ�� �ް��� �۾� �ڵ� �ֱ�
                Debug.Log($"���� �� ���� : {roomInfos.Count} \n ���� �� �̸� : {roomInfos[0].Name}");

                // �������� ������Ʈ �������ֱ�
                Destroy(roomPoller);
            }
        );
    }


    #endregion


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

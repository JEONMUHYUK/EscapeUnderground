using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System;
using ExitGames.Client.Photon;
using System.Runtime.CompilerServices;
using UnityEngine.Tilemaps;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [Header("[ Panels ]")]
    [SerializeField] private Image createRoomPanel = null;
    [SerializeField] private Image findRoomPanel = null;
    [SerializeField] private Image roomPanel = null;
    [SerializeField] private Transform lobbyObjectsPanel = null;

    [Header("[ InputFields ]")]
    [SerializeField] private TMP_InputField roomNameInputField = null;
    [SerializeField] private TMP_InputField findRoomNameInputField = null;

    [Header("[ ScrollView ]")]
    [SerializeField] private GridLayoutGroup roomListPanel;

    [Header("[ Texts ]")]
    [SerializeField] private TextMeshProUGUI cntPlayers;
    [SerializeField] private TextMeshProUGUI nickNameText = null;
    [SerializeField] private TextMeshProUGUI amountText = null;

    [Header("[ Buttons ]")]
    [SerializeField] private Button openCreateRoomBtn = null;     // 방 생성패널 열기 버튼
    [SerializeField] private Button CreateRoomBtn = null;     // 방 생성패널 열기 버튼
    [SerializeField] private Button joinOrCreateRoomBtn = null;     // 방 접속 혹은 생성 버튼
    [SerializeField] private Button findEnterRoomBtn = null;     // 방 찾기 버튼
    [SerializeField] private Button returnLobbyInCRBtn = null;     // 방 찾기 버튼

    [Header("[ TileMaps ]")]
    [SerializeField] private Tilemap miniPanelTileMap = null;

    MyWallet myWallet = null;   // myWallet
    // 사운드 매니저
    AudioManager audioManager = null;

    DappxAPIDataConroller dappxAPIDataConroller = null;
    private void OnEnable()
    {
        Init();
        //버튼 연결.
        openCreateRoomBtn.onClick.AddListener(delegate { OnClickActiveCreateRoomPanel(); audioManager.SoundPlay(audioManager.ClickSound); });
        CreateRoomBtn.onClick.AddListener(delegate { OnClickCreateRoom(); audioManager.SoundPlay(audioManager.ClickSound); });
        joinOrCreateRoomBtn.onClick.AddListener(delegate { OnClickJoinOnCreateRoom(); audioManager.SoundPlay(audioManager.ClickSound); });
        findEnterRoomBtn.onClick.AddListener(delegate { OnClickOpenFindRoomPanel(); audioManager.SoundPlay(audioManager.ClickSound); });
        returnLobbyInCRBtn.onClick.AddListener(delegate { OnClickCloseCreateRoomPanel(); audioManager.SoundPlay(audioManager.ClickSound); });
        // InputField 연결
        roomNameInputField.onEndEdit.AddListener(delegate (string roomName) { SetRoomName(roomName); audioManager.SoundPlay(audioManager.ClickSound); });
        findRoomNameInputField.onEndEdit.AddListener(delegate (string roomName) { SetFindRoomName(roomName); audioManager.SoundPlay(audioManager.ClickSound); });
    }

    void Init()
    {
        audioManager = FindObjectOfType<AudioManager>();
        myWallet = FindObjectOfType<MyWallet>();
        dappxAPIDataConroller = FindObjectOfType<DappxAPIDataConroller>();
        findRoomPanel.gameObject.SetActive(false);
        createRoomPanel.gameObject.SetActive(false);
        roomPanel.gameObject.SetActive(false);
        miniPanelTileMap.gameObject.SetActive(false);
        nickNameText.text = "Player : " + PhotonNetwork.NickName;
        //StartCoroutine(BringAmount());
        Invoke("RenderAmount", 1f);
    }

    void RenderAmount() => amountText.text = "Amount : " + dappxAPIDataConroller.BetSettings.data.bets[0].amount;

    IEnumerator BringAmount()
    {
        yield return dappxAPIDataConroller.BetSettings.data.bets[0].amount > 0;
        amountText.text = "Amount : " + dappxAPIDataConroller.BetSettings.data.bets[0].amount;
    }

    private void Update()
    {
        // 현재 들어온 플레이어 수 업데이트
        cntPlayers.text = "Players : " + PhotonNetwork.CountOfPlayers.ToString();
    }

    #region FindRoomMethod

    // FindRoomPanel 열기
    void OnClickOpenFindRoomPanel()
    {
        miniPanelTileMap.gameObject.SetActive(true);
        findRoomPanel.gameObject.SetActive(true);
        miniPanelTileMap.gameObject.SetActive(true);
        lobbyObjectsPanel.gameObject.SetActive(false);
    } 
    // FindRoomPanel 닫기
    void OnClickCloseFindRoomPanel() 
    {
        audioManager.SoundPlay(audioManager.ClickSound);
        findRoomPanel.gameObject.SetActive(false);
        miniPanelTileMap.gameObject.SetActive(false);
        lobbyObjectsPanel.gameObject.SetActive(true);
    } 


    // 입력한 룸으로 접속.
    void OnClickFindEnterRoom()
    {
        audioManager.SoundPlay(audioManager.ClickSound);
        if (findRoomNameInputField.text.Length == 0) return;
        PhotonNetwork.JoinRoom(findRoomNameInputField.text);
    }
    // 방 이름 입력 받는 함수.
    void SetFindRoomName(string roomName) => findRoomNameInputField.text = roomName;
    #endregion

    // 방이 있으면 Join 없으면 방 생성
    public void OnClickJoinOnCreateRoom()
    {
        if (!PhotonNetwork.InLobby) return;
        // 룸이 존재한다면 랜덤 접속
        if (PhotonNetwork.CountOfRooms > 0)
        {
            PhotonNetwork.JoinRandomRoom();
            return;
        }
        OnClickActiveCreateRoomPanel();
    }

    // 로비 접속 해제.
    public void OnClickLeaveLobby()
    {
        if (!PhotonNetwork.InLobby) return;
        PhotonNetwork.LeaveLobby();
    }

    // 방생성 패널 활성화.
    public void OnClickActiveCreateRoomPanel()
    {
        miniPanelTileMap.gameObject.SetActive(true);
        createRoomPanel.gameObject.SetActive(true);
        lobbyObjectsPanel.gameObject.SetActive(false);
    } 


    // 방 생성
    void OnClickCreateRoom()
    {
        if(!PhotonNetwork.InLobby) return ;
        if (roomNameInputField.text.Length < 1 || roomNameInputField.text.Length > 6) return;
        // 배팅 금액 베팅세팅의 amount로 설정된다.
        float cost = dappxAPIDataConroller.BetSettings.data.bets[0].amount;

        // 제라 화폐로 코스트를 지불할 수 없다면 리턴.
        if (dappxAPIDataConroller.ZeraBalanceInfo.data.balance < cost) return;



        // 룸 옵션 설정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        //// 룸 안에서의 프로퍼티를 정의.
        //roomOptions.CustomRoomProperties = new Hashtable() { { "Cost", cost } };
        //// 로비로 내려줄 프로퍼티를 정해주어야 한다.
        //// 형식은 string의 배열 형태로 CustomRoomPropertiesForLobby에 담아주어야 로비에서 받아 사용이 가능하다.
        //string[] CustomPropertiesListForLobby = new string[] { "Cost" };
        //roomOptions.CustomRoomPropertiesForLobby = CustomPropertiesListForLobby;

        // 재정의된 룸옵션을 룸에 담아준다.
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions) ;
    }

    // 방생성 패널에서 로비로
    void OnClickCloseCreateRoomPanel()
    { 
        createRoomPanel.gameObject.SetActive(false);
        miniPanelTileMap.gameObject.SetActive(false);
        lobbyObjectsPanel.gameObject.SetActive(true);
    } 
    public void SetRoomName(string name) => roomNameInputField.text = name;

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using TMPro;

// brick�� HP�� ��ü �ջ��� �Լ�
public delegate void HpManager();
public class BrickListManager : MonoBehaviourPunCallbacks
{

    public List<Brick> listBrick = new List<Brick>();
    [SerializeField] private float fullHP;
    [SerializeField] private float fullCurHP;
    [SerializeField] private UIManager uiManager;


    // �ִ� ü�°� ���� ü���� �Ѱ��� ������Ƽ
    public float FullHP { get { return fullHP; } }
    public float FullCurHP { get { return fullCurHP; } }

    public HpManager hpManager;

    [SerializeField] AudioManager audioManager = null;
    [SerializeField] LoadSceneStart loadSceneStart = null;
    // �𿢽� api �� ����
    [SerializeField] DappxAPIDataConroller dappxAPIDataConroller;

    [SerializeField] string sessionId = null;

    public string SessionID { get { return sessionId; } }

    

    private void Awake()
    {
        dappxAPIDataConroller = FindObjectOfType<DappxAPIDataConroller>();
        audioManager = FindObjectOfType<AudioManager>();
        hpManager = MaxHP;
    }
    private void OnEnable()
    {
        loadSceneStart = FindObjectOfType<LoadSceneStart>();
        uiManager.SildbarSeting();
    }
    private void Start()
    {
        fullCurHP = fullHP;

        // ���� ��ü�� sseionID �Ҵ�. �ƴ϶�� ����
        if (photonView.IsMine) sessionId = dappxAPIDataConroller.GetSessionID.sessionId;
        else return;

        // ��� ������ ���� ��ü�� SessionID�� �����Ѵ�.
        photonView.RPC("SetSessionID", RpcTarget.Others, dappxAPIDataConroller.GetSessionID.sessionId);
        
        // ������ Ŭ���̾�Ʈ�� ���� ���� ����ó��.
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("IsMasterClient : " + PhotonNetwork.IsMasterClient);

            // ��ŸƮ������ 2�ʵڿ� ����. �÷��̾� ���� �� ��������ȭ�� ��ٷ��ش�.
            Invoke("StartBetting", 1f);
        }

    }

    // ----------------------------------���ð���-----------------------------------------------------
    [PunRPC]
    void SetSessionID(string sessionId)
    {
        this.sessionId = sessionId;
        
        Debug.Log("SessionId In SetSessionID : " + this.sessionId);
    }

    void StartBetting()
    {
        Debug.Log("################## StartBettings");
        // 2�� �ڿ� SetSessionIDArr �� �����Ѵ�.
        Invoke("SetSessionIDArr", 1f);
    }

    void SetSessionIDArr()
    {
        Debug.Log("################## SetSesstionIDArr");

        // �÷��̾��� SessionID�� �޾ƿ��� ���� �迭�� ã���ش�.
        BrickListManager[] brickListManager = FindObjectsOfType<BrickListManager>();

        Debug.Log("BrickListManager[] : " + dappxAPIDataConroller.GetSessionID.sessionId + " / " + brickListManager[1].sessionId);

        // �÷��̾���� SessionId�� ���� �迭 ��ü�� �����Ѵ�.
        // sessionID[0]�� ���� ��ü ���� ���̵� �Ҵ��Ѵ�.
        // sessionID[1]�� ��� ��ü ���� ���̵� �Ҵ��Ѵ�.
        string[] sessionId = new string[2];
        sessionId[0] = brickListManager[0].SessionID;
        Debug.Log("######## # ######## sessionId[0] : " + sessionId[0]);
        sessionId[1] = brickListManager[1].SessionID;
        Debug.Log("######## # ######## sessionId[1] : " + sessionId[1]);

        // �迭�� ���ڷ� �Ѱ��ش�.
        // ���� ����.
        dappxAPIDataConroller.BettingCoinToZera(sessionId);
    }
    // ---------------------------------------------------------------------------------------------------------


    private void Update()
    {
        // �濡�� �÷��̾ �ߵ��� ������ ��
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            EndGame(0, audioManager.WinSound);


        if (listBrick.Count <= 0)
        {
            if (photonView.IsMine)
                EndGame(0, audioManager.WinSound);
            if (!photonView.IsMine)
                EndGame(1, audioManager.LoseSound);
        }
    }

    void EndGame(int trigger, AudioClip clip)
    {
        if (uiManager.WinTextProperty != null || uiManager.LoseTextProperty != null)
            if (uiManager.WinTextProperty.IsActive() || uiManager.LoseTextProperty.IsActive()) return;

        audioManager.BGMSound(clip);
        if (trigger == 0)
        {
            // ���� �� ȸ��
            dappxAPIDataConroller.BettingZara_DeclareWinner();

            uiManager.ShowWinText();
            Invoke("LoadWin", 3f);
        }
        if (trigger == 1)
        {
            uiManager.ShowLoseText();
            Invoke("LoadLose", 3f);
        }
    }

    void LoadLose()
    {
        photonView.RPC("LoadStartScene", RpcTarget.All);
    }
    void LoadWin()
    {
        photonView.RPC("LoadStartScene", RpcTarget.All);
    }

    // ������ Brick�� List�� ����ش�
    public void AddBrick(Brick brick)
    {
        listBrick.Add(brick);
    }
    // List�� ����� Brick�� HP ���հ踦 ���Ѵ�
    public void MaxHP()
    {
        foreach (Brick brick in listBrick)
        {
            fullHP += brick.MaxHP;
        }
    }
    public void CallReceveDamage(float damage)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("ReceiveDamage", RpcTarget.All, damage);
        }
    }

    // �浹�� Slider�� ����
    [PunRPC]
    public void ReceiveDamage(float Damage) => uiManager.CallUpdateHPSlider(Damage);

    // ������ ������ ��� Ŭ���̾�Ʈ�鿡�� LoadStartScene�� ȣ���Ѵ�.
    // �÷��̾� ��ΰ� ���� ������ �Ǹ� OnLeftRoom�� �ݹ��� �Ǵµ� 
    // �� ������ StartScene�� �ε巹�� ��Ų��.
    [PunRPC]
    void LoadStartScene()
    {
        if (!PhotonNetwork.InRoom) return;
        PhotonNetwork.LeaveRoom();
    }
}

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

    [SerializeField] string sessionId = null;

    // �ִ� ü�°� ���� ü���� �Ѱ��� ������Ƽ
    public float FullHP { get { return fullHP; } }
    public float FullCurHP { get { return fullCurHP; } }

    public HpManager hpManager;

    [SerializeField] AudioManager audioManager = null;
    [SerializeField] LoadSceneStart loadSceneStart = null;
    // �𿢽� api �� ����
    [SerializeField] DappxAPIDataConroller dappxAPIDataConroller;


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

        Invoke("StartBetting", 2f);
    }

    // ----------------------------------���ð���-----------------------------------------------------
    [PunRPC]
    void SetSessionID(string sessionId)
    {
        this.sessionId = sessionId;
    }

    void StartBetting()
    {
        Debug.Log("################## StartBettings");
        // ��ü�� ���Ǿ��̵� ����
        if (photonView.IsMine)
            photonView.RPC("SetSessionID", RpcTarget.Others, dappxAPIDataConroller.GetSessionID.sessionId);

        // ������ Ŭ���̾�Ʈ�� ����.
        if (PhotonNetwork.IsMasterClient) Invoke("SetSessionIDArr", 1f);
    }

    void SetSessionIDArr()
    {
        Debug.Log("################## SetSesstionIDArr");
        BrickListManager[] brickListManager = FindObjectsOfType<BrickListManager>();
        string[] sessionId = new string[2];

        sessionId[0] = brickListManager[0].SessionID;
        sessionId[1] = brickListManager[1].SessionID;

        //dappxAPIDataConroller.SessionIdArr = sessionId;
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

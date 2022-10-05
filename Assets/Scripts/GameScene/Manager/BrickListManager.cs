using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using TMPro;
using System.Security.Cryptography;
using Photon.Realtime;

// brick�� HP�� ��ü �ջ��� �Լ�
public delegate void HpManager();
public class BrickListManager : MonoBehaviourPunCallbacks,IInRoomCallbacks
{

    public List<Brick> listBrick = new List<Brick>();
    [SerializeField] private float fullHP;
    [SerializeField] private float fullCurHP;
    [SerializeField] private UIManager uiManager;
    [SerializeField] bool changedMasterClinet = false;

    // �ִ� ü�°� ���� ü���� �Ѱ��� ������Ƽ
    public float FullHP { get { return fullHP; } }
    public float FullCurHP { get { return fullCurHP; } }

    public HpManager hpManager;

    [SerializeField] AudioManager audioManager = null;
    [SerializeField] LoadSceneStart loadSceneStart = null;



    // DappXAPI---------------------------------------------------------
    // �𿢽� api �� ����
    [SerializeField] DappxAPIDataConroller dappxAPIDataConroller;
    [SerializeField] BrickListManager[] brickListManager = null;
    [SerializeField] string sessionId = null;
    [SerializeField] string userProfileId = null;
    [SerializeField] string[] userProfileIds = new string[2];
    public string SessionID { get { return sessionId; } }
    public string UserProfileID { get { return userProfileId; } }

    

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
        if (photonView.IsMine)
        { 
            sessionId = dappxAPIDataConroller.GetSessionID.sessionId;
            userProfileId = dappxAPIDataConroller.GetUserProfile.userProfile._id;
        } 
        else return;

        // ��� ������ ���� ��ü�� SessionID�� �����Ѵ�.
        photonView.RPC("SetSessionID", RpcTarget.Others, dappxAPIDataConroller.GetSessionID.sessionId);
        // ��� ������ ���� ��ü�� userProfile_id�� �����Ѵ�.
        photonView.RPC("SetUserProfileID", RpcTarget.Others, userProfileId);
        
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
    [PunRPC]
    void SetUserProfileID(string userProfile_id)
    {
        this.userProfileId = userProfile_id;
        Debug.Log("SessionId In SetSessionID : " + this.userProfileId);
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
        brickListManager = FindObjectsOfType<BrickListManager>();

        Debug.Log("BrickListManager[] : " + dappxAPIDataConroller.GetSessionID.sessionId + " / " + brickListManager[1].sessionId);

        // �÷��̾���� SessionId�� ���� �迭 ��ü�� �����Ѵ�.
        // sessionID[0]�� ���� ��ü ���� ���̵� �Ҵ��Ѵ�.
        // sessionID[1]�� ��� ��ü ���� ���̵� �Ҵ��Ѵ�.
        // UserProfileID[0]�� ���� ��ü userId�� �Ҵ��Ѵ�.
        // UserProfileID[1]�� ��� ��ü userId�� �Ҵ��Ѵ�.
        string[] sessionId = new string[2];
        string[] userIds = new string[2];
        sessionId[0] = brickListManager[0].SessionID;
        userIds[0] = brickListManager[0].UserProfileID;
        sessionId[1] = brickListManager[1].SessionID;
        userIds[1] = brickListManager[1].UserProfileID;

        photonView.RPC("SetUserIds", RpcTarget.All, userIds);

        Debug.Log("######## # ######## sessionId[0] : " + sessionId[0]);
        Debug.Log("######## # ######## sessionId[1] : " + sessionId[1]);
        Debug.Log("######## # ######## userIds[0] : " + userIds[0]);
        Debug.Log("######## # ######## userIds[1] : " + userIds[1]);

        // �迭�� ���ڷ� �Ѱ��ش�.
        // ���� ����.
        userProfileIds = userIds;
        Debug.Log("######## # ######## userProfileIds[0] : " + userProfileIds[0]);
        Debug.Log("######## # ######## userProfileIds[1] : " + userProfileIds[1]);
        dappxAPIDataConroller.BettingCoinToZera(sessionId);
    }
    public void CallSetBettingId(string betting_id)
    {
        // �ٸ� ������ �ڽŵ鿡�� betting Id�� �Ҵ��ϰ� �Ѵ�.
        photonView.RPC("SetBettingID", RpcTarget.All, betting_id);
        Debug.Log("######## bettingID : " + betting_id);

    }

    [PunRPC]
    void SetBettingID(string betting_id)
    {
        dappxAPIDataConroller.betting_id = betting_id;
        Debug.Log("######## bettingID : " + dappxAPIDataConroller.betting_id);

    }
    // ---------------------------------------------------------------------------------------------------------

    [PunRPC]
    void SetUserIds(string[] userIds)
    {
        dappxAPIDataConroller.userProfileID = userIds;
    }

    private void Update()
    {
        // �濡�� �÷��̾ �ߵ��� ������ ��
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2 && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(StartEndGame(0, audioManager.WinSound));
            return;
        }
 
        if (listBrick.Count <= 0)
        {
            if (photonView.IsMine)
                EndGame(0, audioManager.WinSound);
            if (!photonView.IsMine)
                EndGame(1, audioManager.LoseSound);
        }
    }


    IEnumerator StartEndGame(int trigger, AudioClip clip)
    {
        yield return new WaitForSeconds(2f);
        EndGame(0, audioManager.WinSound);
    }
    void EndGame(int trigger, AudioClip clip)
    {
        if (uiManager.WinTextProperty != null || uiManager.LoseTextProperty != null)
            if (uiManager.WinTextProperty.IsActive() || uiManager.LoseTextProperty.IsActive()) return;

        audioManager.BGMSound(clip);
        
        if (trigger == 0)
        {
            // ���Ӽ����� �÷�� 2�� �̸��̰� ������Ŭ���̾�Ʈ�� ����Ǿ��ٸ�
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2 && changedMasterClinet)
            {
                // ������ ������Ŭ���̾�Ʈ�� �ƴ� Ŭ���̾�Ʈ�� �ε����� ȣ���Ͽ� �ش�.
                // �Ʒ� �Լ����� ������� �ʰ� return ���ش�.
                dappxAPIDataConroller.BettingZara_DeclareWinner(0);
                uiManager.ShowWinText();
                Invoke("LoadWin", 3f);
                return;
            }
            // ���ڰ� ���� �� ���ñ� ȸ��
            Debug.Log("UserPrfileIDs : " + dappxAPIDataConroller.userProfileID[1]);

            uiManager.ShowWinText();
            Invoke("LoadWin", 3f);

            if (!PhotonNetwork.IsMasterClient) return;
            dappxAPIDataConroller.BettingZara_DeclareWinner(1);
        }
        if (trigger == 1)
        {
            // ���ڰ� ����� �� ���ñ� ȸ��
            Debug.Log("UserPrfileIDs : " + dappxAPIDataConroller.userProfileID[0]);
            uiManager.ShowLoseText();
            Invoke("LoadLose", 3f);

            if (!PhotonNetwork.IsMasterClient) return;
            dappxAPIDataConroller.BettingZara_DeclareWinner(0);
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

    void OnMasterClientSwitched(Player newMasterClient)
    {
        changedMasterClinet = true;
    }
}

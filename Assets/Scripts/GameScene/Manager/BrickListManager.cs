using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;

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

    public Ball myBall;

    bool endGame = false;
    bool winGame = false;
    bool loseGame = false;
    GameSceneAudioManager audioManager = null;
    Ball ball = null;
    private void Awake()
    {
        ball = transform.GetComponentInChildren<Ball>();
        endGame = false;
        audioManager = FindObjectOfType<GameSceneAudioManager>();
        hpManager = MaxHP;
        //uiManager.SliderBarSetting();
        uiManager.SildbarSeting();
        myBall = FindObjectOfType<Ball>();
    }
    private void Start()
    {
        fullCurHP = fullHP;
    }
    private void Update()
    {
        if (endGame) return;
        // �濡�� �÷��̾ �ߵ��� ������ ��
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            Debug.Log("EndGame 0");
            EndGame(0, audioManager.WinSound);
        }

        if (listBrick.Count <= 0 && !endGame)
        {
            Debug.Log("EndGame 1" + endGame);
            if (photonView.IsMine)
            {
                Debug.Log("IsMine : " + endGame);
                EndGame(0, audioManager.WinSound);
                endGame = true;
            }
            if (!photonView.IsMine)
            {
                Debug.Log("IsMine None : " + endGame);
                EndGame(1, audioManager.LoseSound);
                endGame = true;
            }
            else
            {
                uiManager.ShowLoseText();
                Invoke("LoadLose", 2f);
            }
            ball.gameObject.SetActive(false);
            
            Debug.Log("EndGame IN Update : " + endGame);
        }

    }

    void EndGame(int trigger, AudioClip clip)
    {
        if (endGame) return;

        Debug.Log("EndGame 2" + endGame );
        audioManager.BGMSound(clip);
        if (trigger == 0)
        {
            
            winGame = true;
            Debug.Log("Win" + endGame + "/" + winGame);
            uiManager.ShowWinText();
            Invoke("LoadWin", 3f);
        }
        else if (trigger == 1)
        {
            loseGame = true;
            Debug.Log("lose" + endGame + "/" + loseGame);
            uiManager.ShowLoseText();
            Invoke("LoadLose", 3f);
        }
    }

    void LoadLose()
    {
        WalletManager.Instance.LoseMoney();
        photonView.RPC("LoadStartScene", RpcTarget.All);
    }
    void LoadWin()
    {
        WalletManager.Instance.GetMoney();
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
            Debug.Log("CallReceveDamage" + damage);
        }
    }

    // �浹�� Slider�� ����
    [PunRPC]
    public void ReceiveDamage(float Damage)
    {
        uiManager.CallUpdateHPSlider(Damage);
        Debug.Log("ReceiveDamage" + Damage);
    }

    // ������ ������ ��� Ŭ���̾�Ʈ�鿡�� LoadStartScene�� ȣ���Ѵ�.
    // �÷��̾� ��ΰ� ���� ������ �Ǹ� OnLeftRoom�� �ݹ��� �Ǵµ� 
    // �� ������ StartScene�� �ε巹�� ��Ų��.
    [PunRPC]
    void LoadStartScene()
    {
        if (!PhotonNetwork.InRoom) return;
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        // �ڱ��ڽ��� ȣ���Ͽ� �濡 ��ũ�� ������� ȣ���Ѵ�.
        PhotonNetwork.LoadLevel("StartScene");
    }
}

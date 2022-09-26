using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    private void Awake()
    {
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
        // �濡�� �÷��̾ �ߵ��� ������ ��
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            uiManager.ShowWinText();
            Invoke("LoadWin", 2f);
        }

        if (listBrick.Count <= 0)
        {
            if (photonView.IsMine)
            {
                uiManager.ShowWinText();
                Invoke("LoadWin", 2f);
            }
            else
            {
                uiManager.ShowLoseText();
                Invoke("LoadLose", 2f);
            }
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

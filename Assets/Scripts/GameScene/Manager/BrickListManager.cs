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

    [SerializeField]  GameSceneAudioManager audioManager = null;
    [SerializeField] LoadSceneStart loadSceneStart = null;

    private void Awake()
    {
        audioManager = FindObjectOfType<GameSceneAudioManager>();
        hpManager = MaxHP;
        uiManager.SildbarSeting();
    }
    private void OnEnable()
    {
        loadSceneStart = FindObjectOfType<LoadSceneStart>();
    }
    private void Start()
    {
        fullCurHP = fullHP;
    }



    private void Update()
    {
        // �濡�� �÷��̾ �ߵ��� ������ ��
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            EndGame(0, audioManager.WinSound);


        if (listBrick.Count <= 0)
        {
            Debug.Log(listBrick.Count);
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
}

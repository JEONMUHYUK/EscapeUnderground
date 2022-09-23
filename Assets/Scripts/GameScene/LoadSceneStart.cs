using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;


public class LoadSceneStart : MonoBehaviourPunCallbacks
{
    [SerializeField] Button loadSceneBtn = null;
    [SerializeField] Button winBtn = null;
    [SerializeField] Button loseBtn = null;

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        loadSceneBtn.onClick.AddListener(delegate 
        {
            LoadStartScene();
        });
        winBtn.onClick.AddListener(delegate
        {
            WalletManager.Instance.GetMoney();
            photonView.RPC("LoadStartScene", RpcTarget.All);
        });
        
        loseBtn.onClick.AddListener( delegate
        {

            WalletManager.Instance.LoseMoney();
            photonView.RPC("LoadStartScene", RpcTarget.All);

        });
    }


    // ������ ������ ��� Ŭ���̾�Ʈ�鿡�� LoadStartScene�� ȣ���Ѵ�.
    // �÷��̾� ��ΰ� ���� ������ �Ǹ� OnLeftRoom�� �ݹ��� �Ǵµ� 
    // �� ������ StartScene�� �ε巹�� ��Ų��.
    [PunRPC]
    void LoadStartScene()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        // �ڱ��ڽ��� ȣ���Ͽ� �濡 ��ũ�� ������� ȣ���Ѵ�.
        PhotonNetwork.LoadLevel("StartScene");
    }

}

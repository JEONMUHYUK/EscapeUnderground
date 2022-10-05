using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;


public class LoadSceneStart : MonoBehaviourPunCallbacks
{

    [SerializeField] AudioManager audioManager = null;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        
    }
    // ������ ������ ��� Ŭ���̾�Ʈ�鿡�� LoadStartScene�� ȣ���Ѵ�.
    // �÷��̾� ��ΰ� ���� ������ �Ǹ� OnLeftRoom�� �ݹ��� �Ǵµ� 
    // �� ������ StartScene�� �ε巹�� ��Ų��.
    private void Start()
    {
        audioManager.BGMSound(audioManager.GameSceneSoundBGM);
    }
    public override void OnLeftRoom()
    {
        if(audioManager != null)
            audioManager.BGMSound(audioManager.StartSceneSoundBGM);
        // �ڱ��ڽ��� ȣ���Ͽ� �濡 ��ũ�� ������� ȣ���Ѵ�.
        PhotonNetwork.LoadLevel("StartScene");
    }

}

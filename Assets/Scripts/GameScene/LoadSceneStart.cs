using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;


public class LoadSceneStart : MonoBehaviourPunCallbacks
{

    // ������ ������ ��� Ŭ���̾�Ʈ�鿡�� LoadStartScene�� ȣ���Ѵ�.
    // �÷��̾� ��ΰ� ���� ������ �Ǹ� OnLeftRoom�� �ݹ��� �Ǵµ� 
    // �� ������ StartScene�� �ε巹�� ��Ų��.

    public override void OnLeftRoom()
    {
        // �ڱ��ڽ��� ȣ���Ͽ� �濡 ��ũ�� ������� ȣ���Ѵ�.
        PhotonNetwork.LoadLevel("StartScene");
    }

}

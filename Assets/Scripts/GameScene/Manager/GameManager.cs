using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    [SerializeField] private bool isGameOver;
    [SerializeField] private bool isSceneChang;

    private void Awake()
    {
        isGameOver = false;
        isSceneChang = false;
    }

    // ���� ���Ḧ Ball���� �˷���
    public bool GetGameOver()
    {
        return isGameOver;
    }
    // FullHP�� 0�̵Ǹ� true�� ����
    public void SetGameOver(bool isgame)
    {
        isGameOver=isgame;
    }

    public bool GetSceneChang()
    {
        return isSceneChang;
    }

    public void SetSceneChang(bool isChang)
    {
        isSceneChang = isChang;
    }
}

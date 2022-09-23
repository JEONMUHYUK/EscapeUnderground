using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    [SerializeField] private bool isGameOver;

    private void Awake()
    {
        isGameOver = false;
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
}

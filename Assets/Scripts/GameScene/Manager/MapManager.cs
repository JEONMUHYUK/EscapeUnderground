using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private bool[,] isPlaced = null;
    Vector2[,] board = null;

    // Brick�� ��ġ�� ��ü board�� Brick�� ������ų ��ġbool
    private void Awake()
    {
        board = new Vector2[7, 5];
        isPlaced = new bool[7, 5];
    }
    // ������ ���� ����
    private void OnEnable()
    {
        int ranNum = Random.Range(0, 3);
        // �迭�� Vector2 ������ ����
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                board[x, y] = new Vector2((-8 + x), y);
            }
        }

        // ���� ����
        if (ranNum == 0)
        {
            CreateMap1(7, 5);
        }
        else if (ranNum == 1)
        {
            CreateMap2(7, 5);
        }
        else
        {
            CreateMap3(7, 5);
        }
        
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (isPlaced[x, y] == false)
                {
                    BrickPool.Instance.GetBrick(board[x, y]);
                }
            }
        }
    }

    // ù��° �� (T�� ��)
    void CreateMap1(int x1, int y1)
    {
        for (int x = 0; x < x1; x++)
        {
            isPlaced[x, 3] = true;
        }
        for (int y = 1; y < y1-1; y++)
        {
            isPlaced[3, y] = true;
        }
    }

    void CreateMap2(int x1, int y1)
    {
        for (int x = 0; x < x1; x++)
        {
            isPlaced[x, 0] = true;
        }
        for (int y = 0; y < y1; y++)
        {
            isPlaced[3, y] = true;
        }
    }

    void CreateMap3(int x1, int y1)
    {
        for (int x = 0; x < x1; x++)
        {
            isPlaced[x, 2] = true;
        }
        for (int y = 0; y < y1; y++)
        {
            isPlaced[1, y] = true;
        }
    }
}

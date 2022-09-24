using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectManager : SingleTon<MapSelectManager>
{
    [SerializeField] int mapSelect;
    // ������ ���� �ε����� ����
    private void Awake()
    {
        mapSelect = Random.Range(0, 3);
    }

    public int GetMapSelect()
    {
        return mapSelect;
    }
}

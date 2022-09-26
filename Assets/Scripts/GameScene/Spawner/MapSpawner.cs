using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviourPunCallbacks
{
    //[SerializeField] private GameObject[] mapPrefab = new GameObject[3];
    [SerializeField] private GameObject playerMap = null;
    [SerializeField] private GameObject challMap = null;
    [SerializeField] int idx;

    BrickListManager BrickListManager;

    List<string> mapPrefabList = new List<string>() { "Prefabs/Map/Map1", "Prefabs/Map/Map2", "Prefabs/Map/Map3" };
    List<Vector3> offsetPos = new List<Vector3>() { new Vector3(-9.5f, -1.5f, 0) , new Vector3(-0.5f, -1.5f, 0) };


    private void Start()
    {
        idx = MapSelectManager.Instance.GetMapSelect();
    }
    private void OnEnable()
    {
        if (PhotonNetwork.IsConnected)
        {
            SetingMap();
        }
    }

    // idx�� ����� ������ ���� ������ position�� ��ġ���� ȣ��
    // ���� Brick HP�� �����ų BrickListManager ȣ��
    public void SetingMap()
    {
        // ��ġ ����ȭ
        if (PhotonNetwork.IsMasterClient)
            playerMap = PhotonNetwork.Instantiate(mapPrefabList[idx], transform.position + offsetPos[0], Quaternion.identity);
        else playerMap = PhotonNetwork.Instantiate(mapPrefabList[idx], transform.position + offsetPos[1], Quaternion.identity);
        BrickListManager = playerMap.GetComponent<BrickListManager>();
        BrickListManager.hpManager();
        
    }
}

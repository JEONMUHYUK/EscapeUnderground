using Photon.Pun;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapSpawner : MonoBehaviourPunCallbacks
{
    //[SerializeField] private GameObject[] mapPrefab = new GameObject[3];
    [SerializeField] private GameObject playerMap = null;
    [SerializeField] private GameObject challMap = null;
    [SerializeField] int idx;

    BrickListManager BrickListManager;

    List<string> mapPrefabList = new List<string>() { "Prefabs/Map/Map1", "Prefabs/Map/Map2", "Prefabs/Map/Map3" };
    List<Vector3> offsetPos = new List<Vector3>() { new Vector3(-1.43f, -0.21f, 0), new Vector3(6.57f, -0.21f, 0) };

    MapSelectManager mapSelectManager = null;

    private void Awake()
    {
        mapSelectManager = FindObjectOfType<MapSelectManager>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            idx = mapSelectManager.GetMapSelect();
            photonView.RPC("SetIdx", RpcTarget.All, idx);
        }
        Invoke("CalllSettingMap", 0.2f);
    }

    void CalllSettingMap()
    {
        if (PhotonNetwork.IsConnected) SetingMap();
    }

    // idx�� ����� ������ ���� ������ position�� ��ġ���� ȣ��
    // ���� Brick HP�� �����ų BrickListManager ȣ��
    public void SetingMap()
    {
        // ��ġ ����ȭ
        if (PhotonNetwork.IsMasterClient)
            playerMap = PhotonNetwork.Instantiate(mapPrefabList[idx], offsetPos[0], Quaternion.identity);
        else playerMap = PhotonNetwork.Instantiate(mapPrefabList[idx], offsetPos[1], Quaternion.identity);
        BrickListManager = playerMap.GetComponent<BrickListManager>();
        BrickListManager.hpManager();
    }

    [PunRPC]
    void SetIdx(int idx) => this.idx = idx;

}

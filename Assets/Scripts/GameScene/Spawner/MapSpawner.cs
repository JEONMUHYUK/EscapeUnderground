using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] mapPrefab = new GameObject[3];
    [SerializeField] private GameObject playerMap = null;
    [SerializeField] private GameObject challMap = null;
    [SerializeField] int idx;

    BrickListManager brickListManager1;
    BrickListManager brickListManager2;

    // MapSelectManager���� ������ ������ ������ idx ����
    private void Awake()
    {
        idx = MapSelectManager.Instance.GetMapSelect();
    }
    private void OnEnable()
    {
        SetingMap();
    }

    // idx�� ����� ������ ���� ������ position�� ��ġ���� ȣ��
    // ���� Brick HP�� �����ų BrickListManager ȣ��
    public void SetingMap()
    {
        playerMap = Instantiate(mapPrefab[idx], transform.position + new Vector3(-9.5f, -1.5f, 0), Quaternion.identity);
        brickListManager1 = playerMap.GetComponent<BrickListManager>();
        brickListManager1.hpManager();

        challMap = Instantiate(mapPrefab[idx], transform.position + new Vector3(-0.5f, -1.5f, 0), Quaternion.identity);
        brickListManager2 = challMap.GetComponent<BrickListManager>();
        brickListManager2.hpManager();
    }
}

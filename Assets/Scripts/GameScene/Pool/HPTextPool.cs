using System.Collections.Generic;
using UnityEngine;

public class HPTextPool : SingleTon<HPTextPool>
{
    [SerializeField] GameObject HPPrefab = null;
    private Queue<GameObject> HPQueue = new Queue<GameObject>();
    [SerializeField] GameObject HPTexts = null;

    // HP Text ����
    private GameObject Create()
    {
        GameObject hp = Instantiate(HPPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        // HP�� �����Ͽ� Canvas�� ��ġ�Ѵ�. ��ġ �ʱ�ȭ
        return hp;
    }

    public GameObject Gethp()
    {
        GameObject hp = null;
        if (HPQueue.Count == 0)
        {
            hp = Create();
        }
        else hp = HPQueue.Dequeue();
        hp.gameObject.SetActive(true);
        hp.transform.SetParent(HPTexts.transform);
        return hp;
    }

    public void Release(GameObject usehp)
    {
        usehp.gameObject.SetActive(false);
        HPQueue.Enqueue(usehp);
    }
}

using UnityEngine;
using Photon.Pun;

public class Brick : MonoBehaviourPun
{
    [SerializeField] private float curHP = 0;  // ������ ���� ü��
    [SerializeField] private float maxHP = 10;  // ������ ������ �ִ� ü��

    [SerializeField] private BrickListManager brickListManager;
    [SerializeField] private UIManager uiManager;

    public float MaxHP { get { return maxHP; } }
    RectTransform textHP;  // HP Text�� ��ġ -> 2D��ǥ �̱� ������ RectTransform���� ����
    
    private void OnEnable()
    {
        textHP = HPTextPool.Instance.Gethp().GetComponent<RectTransform>();
        curHP = maxHP;  // ���۽� ���� ü�°��� ������ �ִ� ü������ ����
        brickListManager.AddBrick(this);
        textHP.transform.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y));  // HP Text�� ��ġ�� ī�޶� �������� ��ȯ
        uiManager.CallUpdateHpText(textHP, curHP); // �ش��ϴ� ������ ü���� UI�� ���
        //uiManager.CallUpdateHPSlider(curHP);
    }

    public void CallReceveDamage(float damage)
    {
        photonView.RPC("ReceiveDamage", RpcTarget.All, damage);
    }


    // �������� �޾��� �� ����Ǵ� �Լ�
    // ü�� 1���� ü���� 0�� �Ǿ��� �� �ش� ������Ʈ ����
    [PunRPC]
    public void ReceiveDamage(float Damage) 
    {
        curHP -= Damage;
        // ��������Ʈ�� photonView.RPC ����
        //uiManager.uiUpdate(textHP, curHP); // �浹�� �� ���� ü���� UI����
        uiManager.CallUpdateHpText(textHP, curHP);
        //uiManager.CallUpdateHPSlider(curHP);
        if (curHP <= 0)
        {
            curHP = 0;

            photonView.RPC("RelaseBrick", RpcTarget.All);
        }
    }

    // Brick ȸ�� �Լ�
    [PunRPC]
    void RelaseBrick()
    {
        brickListManager.listBrick.Remove(this);
        Debug.Log(brickListManager.listBrick.Count);
        BrickPool.Instance.Release(this);
        HPTextPool.Instance.Release(textHP.gameObject);
    }
}

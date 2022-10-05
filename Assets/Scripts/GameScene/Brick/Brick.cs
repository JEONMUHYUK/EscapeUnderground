using UnityEngine;
using Photon.Pun;

public class Brick : MonoBehaviourPun
{
    [SerializeField] private float curHP = 0;  // ������ ���� ü��
    [SerializeField] private float maxHP = 10;  // ������ ������ �ִ� ü��
    [SerializeField] private BrickListManager brickListManager;
    [SerializeField] private UIManager uiManager;

    public float MaxHP { get { return maxHP; } }
    public float CurHP { get { return curHP; } }
    Transform textHP;  // HP Text�� ��ġ -> 2D��ǥ �̱� ������ RectTransform���� ����

    private void OnEnable()
    {
        brickListManager = FindObjectOfType<BrickListManager>();
        uiManager = FindObjectOfType<UIManager>();
        curHP = maxHP;  // ���۽� ���� ü�°��� ������ �ִ� ü������ ����
        brickListManager.AddBrick(this);
    }

    public void CallReceveDamage(float damage)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("ReceiveDamage", RpcTarget.All, damage);
        }
    }

    // �������� �޾��� �� ����Ǵ� �Լ�
    // ü�� 1���� ü���� 0�� �Ǿ��� �� �ش� ������Ʈ ����
    [PunRPC]
    public void ReceiveDamage(float Damage)
    {
        curHP -= Damage;
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
        BrickPool.Instance.Release(this.gameObject);
    }
}

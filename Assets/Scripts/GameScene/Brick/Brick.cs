using UnityEngine;
using Photon.Pun;

public class Brick : MonoBehaviourPun
{
    [SerializeField] private float curHP = 0;  // ������ ���� ü��
    [SerializeField] private float maxHP = 10;  // ������ ������ �ִ� ü��

    [SerializeField] private BrickListManager brickListManager;
    [SerializeField] private UIManager uiManager;

    public float MaxHP { get { return maxHP; } }
    Transform textHP;  // HP Text�� ��ġ -> 2D��ǥ �̱� ������ RectTransform���� ����

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        brickListManager = FindObjectOfType<BrickListManager>();
        uiManager = FindObjectOfType<UIManager>();
        textHP = HPTextPool.Instance.Get(Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y,transform.position.z))).GetComponent<RectTransform>(); // HP Text�� ��ġ�� ī�޶� �������� ��ȯ
        curHP = maxHP;  // ���۽� ���� ü�°��� ������ �ִ� ü������ ����
        brickListManager.AddBrick(this);
        uiManager.CallUpdateHpText(textHP, curHP); // �ش��ϴ� ������ ü���� UI�� ���
    }

    public void CallReceveDamage(float damage)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("ReceiveDamage", RpcTarget.All, damage);
            Debug.Log("�ش� ���� �Ǵ�" +this.curHP);
            Debug.Log("(Brick)CallReceveDamage" + damage);
        }
    }

    // �������� �޾��� �� ����Ǵ� �Լ�
    // ü�� 1���� ü���� 0�� �Ǿ��� �� �ش� ������Ʈ ����
    [PunRPC]
    public void ReceiveDamage(float Damage)
    {
        curHP -= Damage;
        Debug.Log("(Brick)ReceiveDamage" + Damage);
        Debug.Log("(Brick)curHP " + curHP);

        uiManager.CallUpdateHpText(textHP, curHP);

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
        BrickPool.Instance.Release(this);
        HPTextPool.Instance.Release(textHP.gameObject);
    }
}

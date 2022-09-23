using UnityEngine;

public class Brick : MonoBehaviour
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
        uiManager.UpdateHPText(textHP, curHP); // �ش��ϴ� ������ ü���� UI�� ���
    }

    // �������� �޾��� �� ����Ǵ� �Լ�
    // ü�� 1���� ü���� 0�� �Ǿ��� �� �ش� ������Ʈ ����
    void ReceiveDamage(float Damage) 
    {
        curHP -= Damage;
        uiManager.UpdateHPText(textHP, curHP); // �浹�� �� ���� ü���� UI����
        if (curHP <= 0)
        {
            curHP = 0;
            BrickPool.Instance.Release(this);
            HPTextPool.Instance.Release(textHP.gameObject);
        }
    }
}

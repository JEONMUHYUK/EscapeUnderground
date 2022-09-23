using System.Collections.Generic;
using UnityEngine;

// brick�� HP�� ��ü �ջ��� �Լ�
public delegate void HpManager();
public class BrickListManager : MonoBehaviour
{
    
	List<Brick> listBrick = new List<Brick>();
	[SerializeField] private float fullHP;
	[SerializeField] private float fullCurHP;
    [SerializeField] private UIManager uiManager;
    
    // �ִ� ü�°� ���� ü���� �Ѱ��� ������Ƽ
    public float FullHP { get { return fullHP; } }
    public float FullCurHP { get { return fullCurHP; } }

    public HpManager hpManager;
    private void Awake()
    {
        hpManager = MaxHP;
        uiManager.SildbarSeting();
    }
    private void Start()
    {
        fullCurHP = fullHP;
    }
    // ��ü ü���� 0�� �Ǹ� ���ӿ��� ȣ��
    private void Update()
    {
        if (fullCurHP <= 0)
        {
            uiManager.GameOverText();
            GameManager.Instance.SetGameOver(true);
        }
    }
    // ������ Brick�� List�� ����ش�
    public void AddBrick(Brick brick)
	{
		listBrick.Add(brick);
	}
    // List�� ����� Brick�� HP ���հ踦 ���Ѵ�
    public void MaxHP()
    {
        foreach (Brick brick in listBrick)
        {
            fullHP += brick.MaxHP;
        }
    }
    // �浹�� Slider�� ����
    void ReceiveDamage(float Damage)
    {
        fullCurHP -= Damage;
        uiManager.UpdateHPSlider(fullCurHP);   // �浹�� �� ���� �����̴� �� ����
    }
}

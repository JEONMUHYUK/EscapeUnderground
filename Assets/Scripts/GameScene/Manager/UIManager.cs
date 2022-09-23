using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider sliHP;
    [SerializeField] private Text textHP;
    [SerializeField] private GameObject gameOverPrefab;
    [SerializeField] private GameObject sliderHpPrefab;
    [SerializeField] private BrickListManager brickListManager;
    
    private void Awake()
    {
        sliHP = FindObjectOfType<Slider>();
    }
    // �ִ�ü�� ������Ʈ
    private void Start()
    {
        sliHP.maxValue = brickListManager.FullHP;
        UpdateHPSlider(brickListManager.FullCurHP);
    }

    public void SildbarSeting()
    {
        GameObject sliderHp = Instantiate(sliderHpPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        sliderHp.transform.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, -5.1f));
    }

    // Brick�� ��ġ�� curHP�� ǥ��
    public void UpdateHPText(RectTransform obj, float curHP)
    {
        obj.GetComponent<Text>().text = curHP.ToString();
    }
    // slider�� ���� ���Ҷ����� ȣ��
    public void UpdateHPSlider(float curHP)
    {
        sliHP.value = curHP;
    }
    // ���ӿ����� �ؽ�Ʈ�� ��� ��Ű�� �Լ�
    public void GameOverText()
    {
        GameObject gameOver = Instantiate(gameOverPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        gameOver.transform.position = Camera.main.WorldToScreenPoint(new Vector2(0, 0));
    }
}

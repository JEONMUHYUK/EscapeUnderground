using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using TMPro;

public class UIManager : MonoBehaviourPun
{
    [SerializeField] private Slider sliHP;
    [SerializeField] private Text textHP;
    [SerializeField] private GameObject WinPrefab;
    [SerializeField] private GameObject sliderHpPrefab;
    [SerializeField] private BrickListManager brickListManager;

    Text birckHPText;

    public Action ShowWinText;
    public Action ShowLoseText;


    private void Awake()
    {
        sliHP = FindObjectOfType<Slider>();
    }
    private void Start()
    {
        ShowWinText = delegate 
        {
            WinText();
            GameManager.Instance.SetGameOver(true);
        };
        ShowLoseText = delegate
        {
            LoseText();
            GameManager.Instance.SetGameOver(true);
        };

    }
    private void Update()
    {
        //GameOverText();
        //GameManager.Instance.SetGameOver(true);
    }
    public void SildbarSeting()
    {
        GameObject sliderHp = Instantiate(sliderHpPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        sliderHp.transform.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, -5.1f));
    }

    
    public void CallUpdateHpText(RectTransform obj, float curHP)
    {
        birckHPText = obj.GetComponent<Text>();
        photonView.RPC("UpdateHPText", RpcTarget.All, curHP);
    }


    [PunRPC]
    // Brick�� ��ġ�� curHP�� ǥ��
    public void UpdateHPText(float curHP)
    {
        birckHPText.text = curHP.ToString();
    }
    // slider�� ���� ���Ҷ����� ȣ��
    
    public void CallUpdateHPSlider(float damage)
    {
        photonView.RPC("UpdateHPSlider", RpcTarget.All, damage);
    }

    [PunRPC]
    public void UpdateHPSlider(float damage)
    {
        sliHP.value -= damage;
    }
    // ���ӿ����� �ؽ�Ʈ�� ��� ��Ű�� �Լ�
    public void WinText()
    {
        Image Panel = GameObject.FindGameObjectWithTag("GameOverPanel").GetComponent<Image>();
        Panel.gameObject.SetActive(true);
        TextMeshProUGUI text = Panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.gameObject.SetActive(true);
        //GameObject gameOver = Instantiate(WinPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        //gameOver.transform.position = Camera.main.WorldToScreenPoint(new Vector2(0, 0));
    }
    public void LoseText()
    {
        Image Panel = GameObject.FindGameObjectWithTag("GameOverPanel").GetComponent<Image>();
        Panel.gameObject.SetActive(true);
        TextMeshProUGUI text = Panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        text.gameObject.SetActive(true);
        //GameObject gameOver = Instantiate(WinPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        //gameOver.transform.position = Camera.main.WorldToScreenPoint(new Vector2(0, 0));
    }
}

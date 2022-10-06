using UnityEngine;
using Photon.Pun;


public class Ball : MonoBehaviourPun
{
    [Header("RigidBody")]
    [SerializeField] private Rigidbody2D rigid2D = null;                // �÷��̾��� rigidbody2D�� ���� ����
    [SerializeField] private Transform arrow = null;                    // �ڽİ�ü�� arrow�� ���� ����
    [SerializeField] private BrickListManager brickListManager;
    
    [Header("Mouse")]
    [SerializeField] Vector3 mouseDir;                      // mouse�� position���� ������ ����

    [Header("Player Seting")]
    [SerializeField] float pushPower;                       // Ball���� ���ϴ� ��
    [SerializeField] bool move;                             // �̵� ���θ� Ȯ���ϴ� bool��
    [SerializeField] float attackPower;                       // Ball�� ���ݷ��� ������ ����
    [SerializeField] float[] wallCount;
    [SerializeField] int idx;

    AudioManager audioManager = null;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        mouseDir = Vector3.zero;
        pushPower = 430;
        move = false;
        attackPower = 10f;
        wallCount = new float[3];
        idx = 0;
        //  ���� ��ü�� ���� ȭ�� Ȱ��ȭ
        if (photonView.IsMine) arrow.gameObject.SetActive(true);
    }
    void Update()
    {
        if (!photonView.IsMine) return;
        if (!move && GameManager.Instance.GetSceneChang()) PlayerMove();           // Ball�� ������
    }

    // Ball�� �̵� �� ȸ��
    private void PlayerMove()
    {
        if (!GameManager.Instance.GetGameOver())
        {
            mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);         // windowȭ�鿡 �������ִ� mousePosition���� ScreenToWorldPoint�Լ��� �̿��� ����Ƽȭ������ �����ش�mouseDir.z = 0f;
            mouseDir.z = 0f;
            Vector3 posVector = mouseDir - transform.position;                      // Ball�� ������ġ�� MousePoint�� ���⺤�͸� ����
            float angle = Mathf.Atan2(posVector.y, posVector.x) * Mathf.Rad2Deg;    // ���⺤�͸� ����� ������ ���Ѵ�

            if (!move)
            {
                if (mouseDir.y > -4.7f)
                {
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);          // ���� ������ �̿��� Z���� ȸ��
                }
                if (Input.GetMouseButtonDown(0))
                {
                    ClickMouse();
                }
            }
        }
    }

    // Ball �߻�
    void ClickMouse()
    {
        move = true;
       
        arrow.gameObject.SetActive(false);
        //rigid2D.AddRelativeForce(Vector2.right * pushPower, ForceMode2D.Force);         // ������Ʈ�� �������� �̵�, ForceMode2D.Force = ������ �ӵ��� �̵�
        rigid2D.AddRelativeForce(Vector2.right * pushPower);
    }

    // �浹�� �±׿� ���� ���ǹ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.gameObject.tag == "Brick")  Hit(collision);

        if (collision.gameObject.tag == "Wall")
        {
            audioManager.SoundPlay(audioManager.BoundSound);
            wallCount[idx] = transform.position.y;
            idx++;
            if (idx >= 2)
            {
                if (wallCount[0] == wallCount[1])
                {
                    rigid2D.AddRelativeForce(Vector2.down * 40);
                    idx = 0;
                }
                else idx = 0;
            }

        }
        if (collision.gameObject.tag == "Place")
        {
            this.rigid2D.velocity = Vector2.zero;           // Ball�� ���鿡 �����ϸ� ����
            move = false;

            //  ���� ��ü�� ���� ȭ�� Ȱ��ȭ
            if (photonView.IsMine) arrow.gameObject.SetActive(true);
        }
    }

    void Hit(Collision2D target)
    {
        if (photonView.IsMine)
        {
            brickListManager.ReceiveDamage(attackPower);
            target.gameObject.GetComponent<Brick>().CallReceveDamage(attackPower);
        }
    }
}

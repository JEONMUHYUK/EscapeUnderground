using UnityEngine;
using Photon.Pun;

public class Brick : MonoBehaviourPun
{
    [SerializeField] private float curHP = 0;  // ������ ���� ü��
    [SerializeField] private float maxHP = 10;  // ������ ������ �ִ� ü��
    [SerializeField] private BrickListManager brickListManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioManager audioSource;
    public float MaxHP { get { return maxHP; } }
    public float CurHP { get { return curHP; } }
    Transform textHP;  // HP Text�� ��ġ -> 2D��ǥ �̱� ������ RectTransform���� ����

    private void OnEnable()
    {
        brickListManager = FindObjectOfType<BrickListManager>();
        uiManager = FindObjectOfType<UIManager>();
        curHP = maxHP;  // ���۽� ���� ü�°��� ������ �ִ� ü������ ����
        brickListManager.AddBrick(this);
        audioSource = FindObjectOfType<AudioManager>();
    }

    public void CallReceveDamage(float damage)
    {
        if (maxHP == 10 && audioSource != null) audioSource.BrickSoundPlay(audioSource.SoilBrokenSound); 
        else if (maxHP == 20 && audioSource != null) audioSource.BrickSoundPlay(audioSource.StoneBrokenSound);
        else audioSource.BrickSoundPlay(audioSource.IronBrokenSound); ;

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
        if (maxHP == 10)
        {
            BrickParticleManager brickManager = SoilParticlePool.Instance.Get(transform.position).GetComponent<BrickParticleManager>();
            brickManager.relaseParticle += delegate { SoilParticlePool.Instance.Release(brickManager.gameObject); };
        }
        else if (maxHP == 20)
        {
            BrickParticleManager brickManager = StoneParticlePool.Instance.Get(transform.position).GetComponent<BrickParticleManager>();
            brickManager.relaseParticle += delegate { StoneParticlePool.Instance.Release(brickManager.gameObject); };
        }
        else
        {
            BrickParticleManager brickManager = IronParticlePool.Instance.Get(transform.position).GetComponent<BrickParticleManager>();
            brickManager.relaseParticle += delegate { IronParticlePool.Instance.Release(brickManager.gameObject); };
        } 
    }
}

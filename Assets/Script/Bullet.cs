
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("공격 설정")]
    public int damage = 1;
    public float lifeTime = 2f;
    public string targetTag = "Enemy";
    public string ownerTag = "Player";

    private PlayerStats playerStats;

    void Start()
    {
        // 플레이어의 능력치 가져오기
        GameObject player = GameObject.FindGameObjectWithTag(ownerTag);

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                damage = playerStats.attackPower;
            }
            else
            {
                Debug.LogWarning("플레이어에게 PlayerStats가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        Debug.Log("총알 공격력: " + damage);

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 다른 트리거에는 반응하지 않음
        if (other.isTrigger) return;

        // 플레이어 자신은 공격하지 않음
        if (other.CompareTag(ownerTag)) return;

        if (other.CompareTag(targetTag))
        {
            // 먼저 보스 전용 체력 확인
            BossHealth bossHp =
                other.GetComponentInParent<BossHealth>();

            if (bossHp != null)
            {
                bossHp.TakeDamage(damage);
                Debug.Log("보스에게 가한 피해: " + damage);
            }
            else
            {
                // 보스가 아니라면 기존 일반 몬스터 체력 처리
                Health hp = other.GetComponentInParent<Health>();

                if (hp != null)
                {
                    hp.TakeDamage(damage);
                    Debug.Log("일반 몬스터에게 가한 피해: " + damage);
                }
            }
        }

        Destroy(gameObject);
    }
}

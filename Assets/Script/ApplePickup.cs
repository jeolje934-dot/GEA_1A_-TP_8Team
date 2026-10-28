
using UnityEngine;

public class ApplePickup : MonoBehaviour
{
    [Header("회복 설정")]
    public int healAmount = 10;

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        // 플레이어인지 확인
        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth == null)
        {
            playerHealth = other.GetComponentInParent<Health>();
        }

        if (playerHealth == null ||
            !other.CompareTag("Player") &&
            !other.transform.root.CompareTag("Player"))
        {
            return;
        }

        // 이미 체력이 가득 차 있다면 먹어도 회복되지 않음
        if (playerHealth.currentHp >= playerHealth.maxHp)
            return;

        collected = true;

        int beforeHp = playerHealth.currentHp;

        playerHealth.currentHp = Mathf.Min(
            playerHealth.currentHp + healAmount,
            playerHealth.maxHp
        );

        Debug.Log(
            $"사과 획득! 체력: {beforeHp} → " +
            $"{playerHealth.currentHp}/{playerHealth.maxHp}"
        );

        // 체력 UI 갱신
        if (playerHealth.hpSlider != null)
        {
            playerHealth.hpSlider.value =
                playerHealth.currentHp;
        }

        Destroy(gameObject);
    }
}

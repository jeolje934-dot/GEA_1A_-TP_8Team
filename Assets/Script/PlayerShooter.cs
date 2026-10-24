
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("공격 설정")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireCooldown = 0.2f;

    [Header("발사 사운드")]
    public AudioClip duckShootSound;

    [Range(0f, 1f)]
    public float shootVolume = 0.8f;

    private AudioSource audioSource;
    private float lastFireTime = -999f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // AudioSource가 없으면 자동으로 추가
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 게임 시작 시 자동 재생 방지
        audioSource.playOnAwake = false;
    }
        private void Start()
    {
        Debug.Log("오디오 테스트 시작");

        if (duckShootSound == null)
        {
            Debug.LogError("Duck Shoot Sound가 연결되지 않았습니다!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource가 없습니다!");
            return;
        }

        audioSource.PlayOneShot(duckShootSound, 1f);
        Debug.Log("오디오 재생 명령 실행 완료");
    }

    public void OnAttack(InputValue value)
    {
        // 공격 버튼을 누르는 동안만 발사
        if (!value.isPressed) return;

        // 레벨업 창 등으로 게임이 멈췄다면 공격하지 않음
        if (Time.timeScale == 0f) return;

        // 공격속도에 따른 발사 간격 유지
        if (Time.time < lastFireTime + fireCooldown) return;

        // 필수 오브젝트 확인
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Bullet Prefab 또는 Fire Point가 연결되지 않았습니다.");
            return;
        }

        lastFireTime = Time.time;

        // 총알 생성
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        // 총알 발사
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("총알에 Rigidbody가 없습니다.");
        }

        // 오리 발사음 재생
        if (audioSource != null && duckShootSound != null)
        {
            audioSource.PlayOneShot(duckShootSound, shootVolume);
        }
    }
}


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
    private bool isFiring = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    public void OnAttack(InputValue value)
    {
        isFiring = value.isPressed;

        Debug.Log("Attack 입력: " + (isFiring ? "누름" : "놓음"));

        if (isFiring)
            TryShoot();
    }

    private void Update()
    {
        // 마우스의 실제 버튼 상태도 확인해 입력이 남는 문제를 방지
        if (Mouse.current == null)
        {
            isFiring = false;
            return;
        }

        isFiring = Mouse.current.leftButton.isPressed;

        if (!isFiring || Time.timeScale == 0f)
            return;

        TryShoot();
    }

    private void TryShoot()
    {
        if (Time.timeScale == 0f)
            return;

        if (Time.time < lastFireTime + fireCooldown)
            return;

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning(
                "Bullet Prefab 또는 Fire Point가 연결되지 않았습니다."
            );
            return;
        }

        Shoot();
    }

    private void Shoot()
    {
        lastFireTime = Time.time;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        else
            Debug.LogWarning("총알에 Rigidbody가 없습니다.");

        if (audioSource != null && duckShootSound != null)
            audioSource.PlayOneShot(duckShootSound, shootVolume);
    }

    private void OnDisable()
    {
        isFiring = false;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("총알")]
    public GameObject bulletPrefab;

    [Header("총구")]
    public Transform muzzle;

    [Header("카메라")]
    public Camera currentCamera;

    [Header("발사 설정")]
    public float bulletSpeed = 30f;

    void Update()
    {
        // 마우스 왼쪽 클릭
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    void Attack()
    {
        // 총알 생성
        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzle.position,
            muzzle.rotation
        );

        // 총알 방향 설정
        bullet.transform.forward = currentCamera.transform.forward;

        // 총알 속도 설정
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = currentCamera.transform.forward * bulletSpeed;
        }

        Debug.Log("총 발사!");
    }
}
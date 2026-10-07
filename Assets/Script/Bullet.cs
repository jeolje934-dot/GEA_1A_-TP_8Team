using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 총알을 앞으로 발사
        rb.linearVelocity = transform.forward * speed;

        // 3초 후 자동 삭제
        Destroy(gameObject, lifeTime);
    }
}

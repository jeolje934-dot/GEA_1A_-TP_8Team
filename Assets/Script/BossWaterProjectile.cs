
using UnityEngine;

public class BossWaterProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifeTime = 4f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position +=
            transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponentInParent<Health>();

            if (health != null)
                health.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}

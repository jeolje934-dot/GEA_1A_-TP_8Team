
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 5;
    public float lifeTime = 5f;

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
        if (other.CompareTag("Enemy"))
            return;

        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();

            if (health != null)
                health.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}

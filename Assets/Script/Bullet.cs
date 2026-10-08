using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 2f;
    public string targetTag = "Enemy";
    public string ownerTag = "Player";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.CompareTag(ownerTag)) return;

        if (other.CompareTag(targetTag))
        {
            Health hp = other.GetComponent<Health>();
            if (hp != null) hp.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
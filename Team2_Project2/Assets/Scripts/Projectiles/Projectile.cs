using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;

    public void Setup(string projectileTage, int projectileDamage)
    {
        tag = projectileTage;
        damage = projectileDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(tag))
        {
            IDamagable target = other.GetComponent<IDamagable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        if (!other.CompareTag("Pickup"))
        {
            Destroy(gameObject);
        }
    }
}

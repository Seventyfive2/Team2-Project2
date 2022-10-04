using UnityEngine;

public class Projectile : MonoBehaviour, ISetup
{
    [HideInInspector] public int damage;

    public virtual void Setup(string projectileTage, int projectileDamage, float range = 1, bool activate = false)
    {
        tag = projectileTage;
        damage = projectileDamage;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(tag))
        {
            IDamagable target = other.GetComponent<IDamagable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        if (!other.CompareTag("Pickup") && !other.CompareTag(tag))
        {
            Destroy(gameObject);
        }
    }
}

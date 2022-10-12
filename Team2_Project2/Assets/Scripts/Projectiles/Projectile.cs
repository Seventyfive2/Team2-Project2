using UnityEngine;

public class Projectile : MonoBehaviour, ISetup
{
    [HideInInspector] public int damage;

    public virtual void Setup(string projectileTag, int projectileDamage, float range = 1, float duration = 1, bool activate = false)
    {
        tag = projectileTag;
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

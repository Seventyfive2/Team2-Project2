using UnityEngine;

public class AOEProjectile : Projectile
{
    [SerializeField] private GameObject aoeEffect;

    public override void Setup(string projectileTage, int projectileDamage, float range, bool activate = false)
    {
        base.Setup(projectileTage, projectileDamage, range);
        aoeEffect.tag = projectileTage;

        aoeEffect.GetComponent<AOEEffect>().Setup(projectileTage, projectileDamage, range);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tag))
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

    private void OnDestroy()
    {
        aoeEffect.transform.parent = null;
        aoeEffect.GetComponent<AOEEffect>().Activate();
    }
}

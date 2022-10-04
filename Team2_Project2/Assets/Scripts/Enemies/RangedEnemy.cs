using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform attackPos;

    public override Collider[] GetTargetsInRange()
    {
        return Physics.OverlapSphere(transform.position, attackRange);
    }

    public override void Attack()
    {
        Projectile projectileValues = Instantiate(projectile, attackPos.position, transform.rotation).GetComponent<Projectile>();
        projectileValues.transform.position = attackPos.position;
        projectileValues.transform.rotation = attackPos.rotation;
        projectileValues.Setup(tag, attackDamage);
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [Header("Ranged Values")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform attackPos;

    public override Collider[] GetTargetsInRange()
    {
        return Physics.OverlapSphere(transform.position, attackRange);
    }

    public override void Attack()
    {
        Vector3 aim = pathfinding.GetTarget().position;
        Vector3 vectorToTarget = aim - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
        Quaternion qt = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = qt;

        Projectile projectileValues = Instantiate(projectile, attackPos.position, transform.rotation).GetComponent<Projectile>();
        projectileValues.transform.position = attackPos.position;
        projectileValues.transform.rotation = attackPos.rotation;
        projectileValues.Setup(tag, attackDamage);

        GetAttackSpeed();
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

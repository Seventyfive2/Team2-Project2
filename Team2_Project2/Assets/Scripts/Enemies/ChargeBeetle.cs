using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBeetle : BaseEnemy
{
    public override Collider[] GetTargetsInRange()
    {
        return Physics.OverlapSphere(transform.position, attackRange);
    }

    public override void Attack()
    {
        //Projectile projectileValues = Instantiate(projectile, attackPos.position, transform.rotation).GetComponent<Projectile>();
        //projectileValues.transform.position = attackPos.position;
        //projectileValues.transform.rotation = attackPos.rotation;
        //projectileValues.Setup(tag, attackDamage);
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

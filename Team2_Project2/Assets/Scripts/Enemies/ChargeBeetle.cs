using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBeetle : BaseEnemy
{
    [SerializeField] private Rigidbody rb;
    private bool isCharging = false;

    [Header("Beetle Stats")]
    public float chargeStartup = .5f;
    public float chargeRecovery = .5f;
    public float chargeDuration = 1;
    public float chargeSpeed = 5f;

    public override Collider[] GetTargetsInRange()
    {
        return Physics.OverlapSphere(GetAttackPosition(), attackRange);
    }

    public override void Attack()
    {
        StartCoroutine(Charge());
        if(isCharging)
        {
            Vector3.MoveTowards(transform.position, transform.forward, attackRange);
        }

        //Projectile projectileValues = Instantiate(projectile, attackPos.position, transform.rotation).GetComponent<Projectile>();
        //projectileValues.transform.position = attackPos.position;
        //projectileValues.transform.rotation = attackPos.rotation;
        //projectileValues.Setup(tag, attackDamage);
    }

    IEnumerator Charge()
    {
        lockedInState = true;
        pathfinding.GetAgent().isStopped = true;

        yield return new WaitForSeconds(chargeStartup);

        /*
        Vector3 direction = (pathfinding.GetTarget().position - transform.position).normalized;

        rb.velocity = direction * chargeSpeed;
        */

        isCharging = true;
        yield return new WaitForSeconds(chargeDuration);
        isCharging = false;

        yield return new WaitForSeconds(chargeRecovery);

        pathfinding.GetAgent().isStopped = false;
        lockedInState = false;

        yield return null;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetAttackPosition(), attackRange);
    }
}

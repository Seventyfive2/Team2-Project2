using System.Collections;
using UnityEngine;

public class ChargeBeetle : BaseEnemy
{
    [SerializeField] private Rigidbody rb;
    private bool isCharging = false;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 direction;

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
    }

    public void FixedUpdate()
    {
        if(isCharging)
        {
            rb.velocity = direction * chargeSpeed * Time.time;
        }
    }

    IEnumerator Charge()
    {
        lockedInState = true;
        pathfinding.GetAgent().isStopped = true;

        
        warning.SetActive(true);

        yield return new WaitForSeconds(chargeStartup);

        targetPosition = pathfinding.GetTargetPosition();
        direction = (targetPosition - transform.position).normalized;
        isCharging = true;

        yield return new WaitForSeconds(chargeDuration);

        isCharging = false;

        yield return new WaitForSeconds(chargeRecovery);

        pathfinding.GetAgent().isStopped = false;
        lockedInState = false;
        warning.SetActive(false);

        GetAttackSpeed();

        yield return null;
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetAttackPosition(), attackRange);
    }
}

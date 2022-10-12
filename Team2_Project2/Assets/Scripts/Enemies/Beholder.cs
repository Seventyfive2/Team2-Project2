using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : BaseEnemy
{
    [SerializeField] private GameObject[] projectiles;

    [SerializeField] private GameObject[] melee;

    [SerializeField] private Transform attackPos;
    [SerializeField] private float meleeRange = 1f;
    [SerializeField] private int meleeDamage = 10;

    [SerializeField] private GameObject meleeWarning;

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

        if (Vector3.Distance(transform.position, pathfinding.GetTarget().position) > meleeRange)
        {
            Projectile projectileValues = Instantiate(GalaxyRandom.GetRandomFromList(projectiles), attackPos.position, transform.rotation).GetComponent<Projectile>();
            projectileValues.transform.position = attackPos.position;
            projectileValues.transform.rotation = attackPos.rotation;
            projectileValues.Setup(tag, attackDamage);
        }
        else
        {
            ISetup setup = Instantiate(GalaxyRandom.GetRandomFromList(melee), transform.position, transform.rotation).GetComponent<ISetup>();
            setup.Setup(tag, meleeDamage, meleeRange, activate: true);
        }

        GetAttackSpeed();
    }

    public override void Update()
    {
        base.Update();

        meleeWarning.SetActive(!(Vector3.Distance(transform.position, pathfinding.GetTarget().position) > meleeRange));
    }

    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}

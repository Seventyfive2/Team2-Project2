using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/New Ability")]
public class AbilitySO : ScriptableObject
{
    public int cost = 20;
    public float cooldown = 10;
    public Sprite cooldownImage;

    public enum HitDetectionStyle { Melee, AroundPlayer, Raycast, Projectile }

    public HitDetectionStyle hitDetectionStyle = HitDetectionStyle.AroundPlayer;
    public int damage = 10;
    public float range = 2;
    public float duration = 5;
    public GameObject projectile;

    public void UseAbility(Transform userTransform, Transform userAttackTransform)
    {
        switch (hitDetectionStyle)
        {
            case HitDetectionStyle.Melee:
            default:
                Collider[] meleeTargets = Physics.OverlapSphere(userAttackTransform.position, range);
                if (meleeTargets.Length != 0)
                {
                    for (int i = 0; i < meleeTargets.Length; i++)
                    {
                        if (meleeTargets[i].transform.GetComponent<IDamagable>() != null && !meleeTargets[i].CompareTag("Player") && !meleeTargets[i].CompareTag("Building"))
                        {
                            meleeTargets[i].transform.GetComponent<IDamagable>().TakeDamage(damage);
                        }
                    }
                }
                break;
            case HitDetectionStyle.AroundPlayer:
                Collider[] targetsInRange = Physics.OverlapSphere(userTransform.position, range);
                if (targetsInRange.Length != 0)
                {
                    for (int i = 0; i < targetsInRange.Length; i++)
                    {
                        if (targetsInRange[i].transform.GetComponent<IDamagable>() != null && !targetsInRange[i].CompareTag("Player") && !targetsInRange[i].CompareTag("Building"))
                        {
                            targetsInRange[i].transform.GetComponent<IDamagable>().TakeDamage(damage);
                        }
                    }
                }
                break;
            case HitDetectionStyle.Raycast:
                RaycastHit[] raycastTargets = Physics.RaycastAll(userAttackTransform.position, userAttackTransform.forward, range);
                if (raycastTargets.Length != 0)
                {
                    for (int i = 0; i < raycastTargets.Length; i++)
                    {
                        if (raycastTargets[i].transform.GetComponent<IDamagable>() != null && !raycastTargets[i].transform.CompareTag("Player") && !raycastTargets[i].transform.CompareTag("Building"))
                        {
                            raycastTargets[i].transform.GetComponent<IDamagable>().TakeDamage(damage);
                        }
                    }
                }
                break;
            case HitDetectionStyle.Projectile:
                if (projectile != null)
                {
                    Projectile projectileValues = Instantiate(projectile, userAttackTransform.position, userTransform.rotation).GetComponent<Projectile>();
                    projectileValues.transform.position = userAttackTransform.position;
                    projectileValues.transform.rotation = userAttackTransform.rotation;
                    projectileValues.Setup("Player", damage);
                }
                break;
        }
    }
}

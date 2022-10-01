using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponSO weapon;
    [SerializeField] private AbilitySO ability;
    [SerializeField] private Transform attackPos;

    private float primaryCooldown;
    private float secondaryCooldown;
    private float abilityCooldown;

    [Header("UI")]
    [SerializeField] private float uiRefreshRate = .167f;
    private PlayerUI ui;

    void Start()
    {
        if(PlayerData.instance.currentWeapon != null)
        {
            weapon = PlayerData.instance.currentWeapon;
        }

        ui = GameObject.Find("Player Canvas").GetComponent<PlayerUI>();

        StartCoroutine(UpdateCooldowns());
    }

    void Update()
    {
        if(primaryCooldown > 0)
        {
            primaryCooldown -= Time.deltaTime;
        }
        if (secondaryCooldown > 0)
        {
            secondaryCooldown -= Time.deltaTime;
        }
    }

    public void PrimaryAttack(CallbackContext context)
    {
        if(primaryCooldown <= 0 && context.phase == InputActionPhase.Performed && weapon != null)
        {
            DoAttack(weapon.primaryAttackStyle, weapon.primaryDamage, weapon.primaryRange, weapon.primaryProjectile);

            primaryCooldown = weapon.primaryAtkSpeed / 1;
        }
    }

    public void SecondaryAttack(CallbackContext context)
    {
        if (secondaryCooldown <= 0 && context.phase == InputActionPhase.Performed && weapon != null)
        {
            DoAttack(weapon.secondaryAttackStyle, weapon.secondaryDamage, weapon.secondaryRange, weapon.secondaryProjectile);

            secondaryCooldown = weapon.secondaryAtkSpeed / 1;
        }
    }

    public void Ability(CallbackContext context)
    {
        if (abilityCooldown <= 0 && context.phase == InputActionPhase.Performed && ability != null)
        {
            ability.UseAbility(transform,attackPos);

            abilityCooldown = ability.cooldown / 1;
        }
    }

    public void DoAttack(WeaponSO.AttackStyle style, int damage, float range, GameObject projectile = null)
    {
        switch (style)
        {
            case WeaponSO.AttackStyle.Melee:
            default:
                Collider[] meleeTargets = Physics.OverlapSphere(GetAttackPositionOffset(range), range);
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
            case WeaponSO.AttackStyle.Raycast:
                RaycastHit[] raycastTargets = Physics.RaycastAll(attackPos.position, attackPos.forward, range);
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
            case WeaponSO.AttackStyle.Projectile:
                if(projectile != null)
                {
                    Projectile projectileValues = Instantiate(projectile, attackPos.position, transform.rotation).GetComponent<Projectile>();
                    projectileValues.transform.position = attackPos.position;
                    projectileValues.transform.rotation = attackPos.rotation;
                    projectileValues.Setup(tag,damage);
                }
                break;
        }
    }

    public void ChangeWeapon(WeaponSO newWeapon)
    {
        weapon = newWeapon;

        PlayerData.instance.currentWeapon = newWeapon;

        primaryCooldown = 0;
        secondaryCooldown = 0;

        ui.ChangeWeapon(newWeapon.primaryCooldownImage, newWeapon.secondaryCooldownImage);
    }

    public Vector3 GetAttackPositionOffset(float range)
    {
        return transform.position + attackPos.forward * range;
    }

    private IEnumerator UpdateCooldowns()
    {
        while(true)
        {
            if(weapon != null)
            {
                ui.UpdateCooldowns(primaryCooldown / weapon.primaryAtkSpeed, secondaryCooldown / weapon.secondaryAtkSpeed, abilityCooldown);
            }
            yield return new WaitForSeconds(uiRefreshRate);
        }
    }

    private void OnDrawGizmos()
    {
        if(weapon != null)
        {
            Gizmos.color = Color.red;
            switch (weapon.primaryAttackStyle)
            {
                case WeaponSO.AttackStyle.Melee:
                default:
                    Gizmos.DrawWireSphere(GetAttackPositionOffset(weapon.primaryRange), weapon.primaryRange);
                    break;
                case WeaponSO.AttackStyle.Raycast:
                    Gizmos.DrawLine(attackPos.position, attackPos.position + Vector3.forward * weapon.primaryRange);
                    break;
                case WeaponSO.AttackStyle.Projectile:
                    break;
            }

            Gizmos.color = Color.blue;
            switch (weapon.secondaryAttackStyle)
            {
                case WeaponSO.AttackStyle.Melee:
                default:
                    Gizmos.DrawWireSphere(GetAttackPositionOffset(weapon.secondaryRange), weapon.secondaryRange);
                    break;
                case WeaponSO.AttackStyle.Raycast:
                    Gizmos.DrawLine(attackPos.position, attackPos.position + attackPos.forward * weapon.secondaryRange);
                    break;
                case WeaponSO.AttackStyle.Projectile:
                    break;
            }
        }

        if(ability != null)
        {
            Gizmos.color = Color.magenta;
            switch (ability.hitDetectionStyle)
            {
                case AbilitySO.HitDetectionStyle.Melee:
                    Gizmos.DrawWireSphere(attackPos.position, ability.range);
                    break;
                case AbilitySO.HitDetectionStyle.AroundPlayer:
                    Gizmos.DrawWireSphere(transform.position, ability.range);
                    break;
                case AbilitySO.HitDetectionStyle.Raycast:
                    Gizmos.DrawLine(attackPos.position, attackPos.position + attackPos.forward * ability.range);
                    break;
                case AbilitySO.HitDetectionStyle.Projectile:
                    break;
                default:
                    break;
            }
        }
    }
}

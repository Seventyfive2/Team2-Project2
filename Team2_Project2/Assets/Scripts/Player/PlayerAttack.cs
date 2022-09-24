using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponSO weapon;
    [SerializeField] private Transform attackPos;

    private float primaryCooldown;
    private float secondaryCooldown;

    [Header("UI")]
    [SerializeField] private float uiRefreshRate = .167f;
    [SerializeField] private Image primaryCooldownFill;
    [SerializeField] private Image secondaryCooldownFill;

    void Start()
    {
        StartCoroutine(UpdateCooldowns());
    }

    // Update is called once per frame
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
        if(primaryCooldown <= 0 && context.phase == InputActionPhase.Performed)
        {
            DoAttack(weapon.primaryAttackStyle, weapon.primaryDamage, weapon.primaryRange, weapon.primaryProjectile);

            primaryCooldown = weapon.primaryAtkSpeed / 1;
        }
    }

    public void SecondaryAttack(CallbackContext context)
    {
        if (secondaryCooldown <= 0 && context.phase == InputActionPhase.Performed)
        {
            DoAttack(weapon.secondaryAttackStyle, weapon.secondaryDamage, weapon.secondaryRange, weapon.secondaryProjectile);

            secondaryCooldown = weapon.secondaryAtkSpeed / 1;
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
                        if (meleeTargets[i].transform.GetComponent<IDamagable>() != null && !meleeTargets[i].CompareTag("Player"))
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
                        if (raycastTargets[i].transform.GetComponent<IDamagable>() != null && !raycastTargets[i].transform.CompareTag("Player"))
                        {
                            raycastTargets[i].transform.GetComponent<IDamagable>().TakeDamage(damage);
                        }
                    }
                }
                break;
            case WeaponSO.AttackStyle.Projectile:
                Instantiate(projectile, attackPos.position, Quaternion.identity);
                break;
        }
    }

    public void ChangeWeapon(WeaponSO newWeapon)
    {
        weapon = newWeapon;

        primaryCooldown = 0;
        secondaryCooldown = 0;
    }

    public Vector3 GetAttackPositionOffset(float range)
    {
        return transform.position + attackPos.forward * range;
    }

    private IEnumerator UpdateCooldowns()
    {
        while(true)
        {
            primaryCooldownFill.fillAmount = primaryCooldown/ weapon.primaryAtkSpeed;
            secondaryCooldownFill.fillAmount = secondaryCooldown/ weapon.secondaryAtkSpeed;
            yield return new WaitForSeconds(uiRefreshRate);
        }
    }

    private void OnDrawGizmos()
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
}

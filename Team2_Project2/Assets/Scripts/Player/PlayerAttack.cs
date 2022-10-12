using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static AbilitySO;
using static ItemSO;
using static PlayerMovement;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAttack : MonoBehaviour
{
    [Header("Equipment")]
    [SerializeField] private WeaponSO weapon;
    [SerializeField] private AbilitySO ability;
    [SerializeField] private ItemStack selectedItem;
    private int itemIndex;
    [SerializeField] private List<ItemStack> items;
    private bool usingItem = false;
    
    private PlayerAttribute playerDamage;

    [Header("Components")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private PlayerMovement movement;
    [Space]
    [SerializeField] private GameObject swingModel;
    [SerializeField] private float effectDuration = .1f;

    [Header("UI")]
    [SerializeField] private float uiRefreshRate = .167f;
    private PlayerUI ui;

    //Events
    public event EventHandler<PlayerAttackEventArgs> OnPlayerAttack;
    public class PlayerAttackEventArgs : EventArgs
    {
        public WeaponSO weapon;

        public PlayerAttackEventArgs(WeaponSO weapon)
        {
            this.weapon = weapon;
        }
    }

    public event EventHandler<PlayerSpecialEventArgs> OnPlayerSpecial;
    public class PlayerSpecialEventArgs : EventArgs
    {
        public AbilitySO ability;
        public PlayerSpecialEventArgs(AbilitySO ability)
        {
            this.ability = ability;
        }
    }

    public event EventHandler<PlayerItemEventArgs> OnPlayerItem;
    public class PlayerItemEventArgs : EventArgs
    {
        public ItemSO item;
        public PlayerItemEventArgs(ItemSO item)
        {
            this.item = item;
        }
    }

    //Cooldown Timers
    private float primaryCooldown;
    private float secondaryCooldown;
    private float abilityCooldown;

    void Start()
    {
        if(PlayerData.instance.currentWeapon != null)
        {
            weapon = PlayerData.instance.currentWeapon;
        }

        if (PlayerData.instance.currentAbility != null)
        {
            ability = PlayerData.instance.currentAbility;
        }

        if (PlayerData.instance.items.Count > 0)
        {
            items = PlayerData.instance.items;
            selectedItem = items[0];
        }

        playerDamage = PlayerData.instance.GetAttribute("Damage");

        ui = GameObject.Find("Player Canvas").GetComponent<PlayerUI>();

        if (ability != null)
        {
            ui.ChangeAbility(ability.cooldownImage);
        }

        if(selectedItem.item != null)
        {
            ui.ChangeItem(selectedItem.item.uiImage, selectedItem.amtHeld);
        }

        //animation GetComponent<Animator>();

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
        if(abilityCooldown > 0)
        {
            abilityCooldown -= Time.deltaTime;
        }
    }

    public void PrimaryAttack(CallbackContext context)
    {
        if(movement.isControllable)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (usingItem)
                {
                    usingItem = false;
                    selectedItem.item.UseItem(transform.position, tag);
                    selectedItem.amtHeld--;
                    ui.ChangeItemAmount(selectedItem.amtHeld);
                }
                else if (primaryCooldown <= 0 && weapon != null)
                {
                    DoAttack(weapon.primaryAttackStyle, GetBoostedDamage(weapon.primaryDamage), weapon.primaryRange, weapon.primaryProjectile);

                    primaryCooldown = weapon.primaryAtkSpeed / 1;
                }
            }
        }
    }

    public void SecondaryAttack(CallbackContext context)
    {
        if(movement.isControllable)
        {
            if (secondaryCooldown <= 0 && context.phase == InputActionPhase.Performed && weapon != null)
            {
                DoAttack(weapon.secondaryAttackStyle, GetBoostedDamage(weapon.secondaryDamage), weapon.secondaryRange, weapon.secondaryProjectile);

                secondaryCooldown = weapon.secondaryAtkSpeed / 1;
            }
        }
    }

    public void Ability(CallbackContext context)
    {
        if (movement.isControllable)
        {
            if (abilityCooldown <= 0 && context.phase == InputActionPhase.Performed && ability != null)
            {
                switch (ability.hitDetectionStyle)
                {
                    case HitDetectionStyle.FrontOfPlayer:
                    default:
                        ability.UseAbility(attackPos.position, tag, GetBoostedDamage(ability.damage));
                        break;
                    case HitDetectionStyle.AroundPlayer:
                        ability.UseAbility(transform.position, tag, GetBoostedDamage(ability.damage), transform);
                        break;
                    case HitDetectionStyle.AtCursor:
                        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                        Vector3 input = Vector3.zero;

                        if (Physics.Raycast(ray, out RaycastHit raycastHit))
                        {
                            input = raycastHit.point;
                        }

                        ability.UseAbility(input, tag, GetBoostedDamage(ability.damage));
                        break;
                }

                if (OnPlayerSpecial != null) OnPlayerSpecial (this, new PlayerSpecialEventArgs(ability));

                abilityCooldown = ability.cooldown / 1;
            }
        }
    }

    public void UseItem(CallbackContext context)
    {
        if(movement.isControllable)
        {
            if (context.phase == InputActionPhase.Performed && selectedItem.item != null)
            {
                if (selectedItem.amtHeld > 0)
                {
                    switch (selectedItem.item.activationMethod)
                    {
                        case ActivationMethod.UseOnPlayer:
                            selectedItem.item.UseItem(transform.position, tag);

                            selectedItem.amtHeld--;
                            ui.ChangeItemAmount(selectedItem.amtHeld);
                            break;
                        case ActivationMethod.UseAtCursor:
                            usingItem = !usingItem;
                            break;
                        default:
                            break;
                    }

                    if (OnPlayerItem != null) OnPlayerItem(this, new PlayerItemEventArgs(selectedItem.item));
                }
            }
        }
    }

    public void SwitchItem(CallbackContext context)
    {
        if (movement.isControllable)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (usingItem)
                {
                    usingItem = false;
                }

                itemIndex++;

                if (itemIndex >= items.Count)
                {
                    itemIndex = 0;
                }

                selectedItem = items[itemIndex];
                ui.ChangeItem(selectedItem.item.uiImage, selectedItem.amtHeld);
            }
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
                
                GameObject swingPrefab = Instantiate(swingModel, GetAttackPositionOffset(range+ .75f), attackPos.rotation);
                swingPrefab.GetComponent<ISetup>().Setup(tag, damage, range, effectDuration, true);

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
                if(projectile != null)
                {
                    Projectile projectileValues = Instantiate(projectile, attackPos.position, attackPos.rotation).GetComponent<Projectile>();
                    //projectileValues.transform.position = attackPos.position;
                    //projectileValues.transform.rotation = attackPos.rotation;
                    if(projectileValues != null)
                    {
                        projectileValues.Setup(tag, damage);
                    }
                }
                break;
        }

        if (OnPlayerAttack != null) OnPlayerAttack(this, new PlayerAttackEventArgs(weapon));
    }

    public void ChangeWeapon(WeaponSO newWeapon, out WeaponSO oldWeapon)
    {
        oldWeapon = weapon;
        weapon = newWeapon;

        PlayerData.instance.currentWeapon = newWeapon;

        primaryCooldown = 0;
        secondaryCooldown = 0;

        ui.ChangeWeapon(newWeapon.primaryCooldownImage, newWeapon.secondaryCooldownImage);
    } 

    public void AddItem(ItemSO newItem, int amt)
    {
        bool stackItems = false;
        ItemStack stackToAdd = null;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == newItem)
            {
                stackItems = true;
                stackToAdd = items[i];
            }
        }

        if(!stackItems)
        {
            items.Add(new ItemStack(newItem, 1));
        }
        else
        {
            stackToAdd.StackChange(amt);
        }
    }

    public int GetBoostedDamage(int damage)
    {
        int result = damage + playerDamage.GetStatIncrease();

        return result; 
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
                if (ability != null)
                {
                    ui.UpdateCooldowns(primaryCooldown / weapon.primaryAtkSpeed, secondaryCooldown / weapon.secondaryAtkSpeed, abilityCooldown / ability.cooldown);
                }
                else
                {

                    ui.UpdateCooldowns(primaryCooldown / weapon.primaryAtkSpeed, secondaryCooldown / weapon.secondaryAtkSpeed, 0);
                }
            }
            yield return new WaitForSeconds(uiRefreshRate);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(attackPos.position, attackPos.position + attackPos.forward);

        if (weapon != null)
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
                case HitDetectionStyle.FrontOfPlayer:
                    Gizmos.DrawWireSphere(attackPos.position, ability.range);
                    break;
                case HitDetectionStyle.AroundPlayer:
                    Gizmos.DrawWireSphere(transform.position, ability.range);
                    break;
                case HitDetectionStyle.AtCursor:
                    Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                    Vector3 input = Vector3.zero;

                    if (Physics.Raycast(ray, out RaycastHit raycastHit))
                    {
                        input = raycastHit.point;
                    }
                    Gizmos.DrawWireSphere(input, .25f);
                    break;
                default:
                    break;
            }
        }
    }
}

[System.Serializable]
public class ItemStack
{
    public string itemName;
    public ItemSO item;
    public int amtHeld;
    public int maxAmt;

    public ItemStack(ItemSO item, int amtHeld)
    {
        this.item = item;
        this.amtHeld = amtHeld;
        this.maxAmt = item.maxHeld;

        itemName = item.name;
    }

    public void StackChange(int amt)
    {
        amtHeld += amt;

        if(amtHeld > maxAmt)
        {
            amtHeld = maxAmt;
        }
    }

    public string GetAmountHeld()
    {
        return "Held " + amtHeld + "/" + maxAmt;
    }

    public bool IsFull()
    {
        return amtHeld >= maxAmt;
    }
}


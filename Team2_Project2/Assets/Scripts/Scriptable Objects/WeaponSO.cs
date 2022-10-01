using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/New Weapon")]
public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public int weaponCost = 20;

    public enum AttackStyle { Melee, Raycast, Projectile}

    [Header("Primary Stats")]
    public AttackStyle primaryAttackStyle = AttackStyle.Melee;
    public int primaryDamage = 5;
    public float primaryRange = 1;
    public float primaryAtkSpeed = 1;
    [ConditionalValue("primaryAttackStyle", Condition2 = "Projectile")] public GameObject primaryProjectile;
    public Sprite primaryCooldownImage;

    [Header("Secondary Stats")]
    public AttackStyle secondaryAttackStyle = AttackStyle.Melee;
    public int secondaryDamage = 10;
    public float secondaryRange = 1;
    public float secondaryAtkSpeed = 1;
    [ConditionalValue("secondaryAttackStyle", Condition2 = "Projectile")] public GameObject secondaryProjectile;
    public Sprite secondaryCooldownImage;
}

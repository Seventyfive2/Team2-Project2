using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private PlayerData playerData;

    [SerializeField] private Transform contentHolder;

    void Start()
    {
        playerData = PlayerData.instance;
    }

    public void BuyWeapon(WeaponSO weapon)
    {
        if(playerData.coins >= weapon.weaponCost)
        {
            playerData.coins -= weapon.weaponCost;
            playerData.currentWeapon = weapon;
        }
    }

    public void BuyAbility(AbilitySO ability)
    {
        if (playerData.coins >= ability.cost)
        {
            playerData.coins -= ability.cost;
            playerData.currentAbility = ability;
        }
    }
}

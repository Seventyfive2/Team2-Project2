using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public int coins;
    public WeaponSO currentWeapon;
    public AbilitySO currentAbility;

    [Header("Player Upgrades")]
    public PlayerAttribute[] attributes;
    public PlayerAttribute damage;
    public PlayerAttribute speed;

    public List<BuildingData> buildingStatus;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LevelEnded()
    {

    }
}

[System.Serializable]
public class BuildingData
{
    public string name;
    public HealthSystem healthSystem;
    public Sprite sprite;

    public BuildingData(string name, HealthSystem buildingHealth, Sprite sprite)
    {
        this.name = name;
        healthSystem = buildingHealth;
        this.sprite = sprite;
    }
}

[System.Serializable]
public class PlayerAttribute
{
    public string name;
    public int upgradeAmt = 0;
    public int maxUpgradeAmt = 5;

    public int costToUpgrade = 25;
    public Sprite shopSprite;
}

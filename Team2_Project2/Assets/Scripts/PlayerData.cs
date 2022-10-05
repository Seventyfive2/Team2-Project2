using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public int coins;
    [Header("Player Equipment")]
    public WeaponSO currentWeapon;
    public AbilitySO currentAbility;

    public ItemStack selectedItem;
    public List<ItemStack> items;

    public string[] nextLevel;
    public int levelIndex = 0;

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

        DontDestroyOnLoad(instance);
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

        if (!stackItems)
        {
            items.Add(new ItemStack(newItem, 1));
        }
        else
        {
            stackToAdd.amtHeld += amt;
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel[levelIndex]);
    }

    public void LevelEnded()
    {
        levelIndex++;
    }

    public void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1;
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

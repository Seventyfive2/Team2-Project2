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

    public int levelsCompleted = 0;

    public ItemStack selectedItem;
    public List<ItemStack> items;

    public PlayerAttribute[] attributes;

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

    public void AddItem(ItemSO newItem, int amt, int maxAmt = 1)
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

    public void LoadNextLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LevelEnded()
    {
        levelsCompleted++;
    }

    public void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1;
    }

    public PlayerAttribute GetAttribute(string attributeName)
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            if (attributes[i].name == attributeName)
            {
                return attributes[i];
            }
        }

        return new PlayerAttribute();
    }

    public void NewGame()
    {
        coins = 0;
        levelsCompleted = 0;

        currentWeapon = null;
        currentAbility = null;
        items.Clear();
        selectedItem = null;

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].upgradeAmt = 0;
        }

        buildingStatus.Clear();
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

    public int statIncreasePerLevel = 5;

    public int costToUpgrade = 25;
    public Sprite shopSprite;

    public string UpgradeLevel()
    {
        return "Level " + upgradeAmt + "/" + maxUpgradeAmt;
    }

    public bool IsMaxed()
    {
        return upgradeAmt >= maxUpgradeAmt;
    }

    public int GetStatIncrease()
    {
        return upgradeAmt * statIncreasePerLevel;
    }
}

using System;
using System.Diagnostics;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private PlayerData playerData;

    [SerializeField] private Transform contentHolder;
    [SerializeField] private Button[] tabs;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private string cointName = "Coins:";

    [SerializeField] private GameObject entryPrefab;

    [SerializeField] private ShopItem<WeaponSO>[] weapons;
    [SerializeField] private ShopItem<AbilitySO>[] abilities;
    [SerializeField] private ShopItem<ItemSO>[] items;

    [SerializeField] private int goldPerHealthMissing = 5;

    [SerializeField] private string[] levelNames;

    void Start()
    {
        playerData = PlayerData.instance;
        
        CoinsChanged();

        ShowTab(0);

        for (int i = 0; i < tabs.Length; i++)
        {
            int temp = i;
            tabs[i].onClick.AddListener(() => ShowTab(temp));
        }
    }


    public void ShowTab(int index)
    {
        for (int i = contentHolder.childCount-1; i >= 0; i--)
        {
            Destroy(contentHolder.GetChild(i).gameObject);
        }

        switch (index)
        {
            case 0:
            default:
                for (int i = 0; i < weapons.Length; i++)
                {
                    CreateWeaponEntry(weapons[i].item.primaryCooldownImage, weapons[i].itemName, weapons[i].item.weaponCost, weapons[i].item);
                }
                break;
            case 1:
                for (int i = 0; i < abilities.Length; i++)
                {
                    CreateAbilityEntry(abilities[i].item.cooldownImage, abilities[i].itemName, abilities[i].item.cost, abilities[i].item);
                }
                break;
            case 2:
                for (int i = 0; i < items.Length; i++)
                {
                    CreateItemEntry(items[i].item.uiImage, items[i].itemName, items[i].item.cost, items[i].item);
                }
                break;
            case 3:
                for (int i = 0; i < playerData.attributes.Length; i++)
                {
                    CreateUpgradeEntry(playerData.attributes[i].shopSprite, playerData.attributes[i].name, playerData.attributes[i].GetCost(), playerData.attributes[i]);
                }
                break;
            case 4:
                for (int i = 0; i < playerData.buildingStatus.Count; i++)
                {
                    CreateBuildingEntry(playerData.buildingStatus[i].sprite, playerData.buildingStatus[i].name, playerData.buildingStatus[i].healthSystem.GetMissingHealth() * goldPerHealthMissing, playerData.buildingStatus[i]);
                }
                break;
        }
    }

    #region Create Shop Entries
    public void CreateWeaponEntry(Sprite sprite, string name, int price, WeaponSO weapon)
    {
        ShopEntry newEntry = Instantiate(entryPrefab, contentHolder).GetComponent<ShopEntry>();

        newEntry.buyButton.onClick.AddListener(() => BuyWeapon(weapon));

        newEntry.Initialize(sprite, name, price);

        if (playerData.currentWeapon == weapon)
        {
            newEntry.buyButton.interactable = false;
            newEntry.itemPrice.text = "Equiped";
        }
    }
    public void CreateAbilityEntry(Sprite sprite, string name, int price, AbilitySO ablitiy)
    {
        ShopEntry newEntry = Instantiate(entryPrefab, contentHolder).GetComponent<ShopEntry>();

        newEntry.buyButton.onClick.AddListener(() => BuyAbility(ablitiy));

        newEntry.Initialize(sprite, name, price);

        if (playerData.currentAbility == ablitiy)
        {
            newEntry.buyButton.interactable = false;
            newEntry.itemPrice.text = "Equiped";
        }
    }
    public void CreateItemEntry(Sprite sprite, string name, int price, ItemSO item)
    {
        ShopEntry newEntry = Instantiate(entryPrefab, contentHolder).GetComponent<ShopEntry>();

        newEntry.buyButton.onClick.AddListener(() => BuyConsumable(item));

        newEntry.Initialize(sprite, name, price, true);

        bool inPlayerInv = false;

        for (int i = 0; i < playerData.items.Count; i++)
        {
            if(playerData.items[i].item == item)
            {
                newEntry.itemAmt.text = playerData.items[i].GetAmountHeld();

                inPlayerInv = true;
                if (playerData.items[i].IsFull())
                {
                    newEntry.buyButton.interactable = false;
                    newEntry.itemPrice.text = "Maxed";
                }

                break;
            }
        }

        if(!inPlayerInv)
        {
            newEntry.itemAmt.text = "Held " + 0 + "/" + item.maxHeld;
        }
    }
    public void CreateUpgradeEntry(Sprite sprite, string name, int price, PlayerAttribute attribute)
    {
        ShopEntry newEntry = Instantiate(entryPrefab, contentHolder).GetComponent<ShopEntry>();

        newEntry.buyButton.onClick.AddListener(() => BuyUpgrade(attribute));

        newEntry.Initialize(sprite, name, price, true);

        for (int i = 0; i < playerData.attributes.Length; i++)
        {
            if(playerData.attributes[i] == attribute)
            {
                newEntry.itemAmt.text = playerData.attributes[i].UpgradeLevel();

                if (playerData.attributes[i].IsMaxed())
                {
                    newEntry.buyButton.interactable = false;
                    newEntry.itemPrice.text = "Max Level";
                }
            }
        }
    }
    public void CreateBuildingEntry(Sprite sprite, string name, int price, BuildingData building)
    {
        ShopEntry newEntry = Instantiate(entryPrefab, contentHolder).GetComponent<ShopEntry>();

        newEntry.buyButton.onClick.AddListener(() => RepairBuilding(building));

        newEntry.slider.value = building.healthSystem.GetHealthPercent();

        newEntry.Initialize(sprite, name, price, false, true);
    }
    #endregion

    #region Buy
    public void BuyWeapon(WeaponSO weapon)
    {
        if(playerData.coins >= weapon.weaponCost)
        {
            playerData.coins -= weapon.weaponCost;
            playerData.currentWeapon = weapon;
            CoinsChanged();
            ShowTab(0);
        }
    }

    public void BuyAbility(AbilitySO ability)
    {
        if (playerData.coins >= ability.cost)
        {
            playerData.coins -= ability.cost;
            playerData.currentAbility = ability;
            CoinsChanged();
            ShowTab(1);
        }
    }

    public void BuyConsumable(ItemSO item)
    {
        if (playerData.coins >= item.cost)
        {
            playerData.coins -= item.cost;
            playerData.AddItem(item, 1, item.maxHeld);
            CoinsChanged();
            ShowTab(2);
        }
    }

    public void BuyUpgrade(PlayerAttribute attribute)
    {
        if(playerData.coins >= attribute.GetCost() && attribute.upgradeAmt < attribute.maxUpgradeAmt)
        {
            playerData.coins -= attribute.GetCost();
            attribute.upgradeAmt++;
            CoinsChanged();
            ShowTab(3);
        }
    }

    public void RepairBuilding(BuildingData data)
    {
        int cost = data.healthSystem.GetMissingHealth() * goldPerHealthMissing;
        if (playerData.coins >= cost)
        {
            data.healthSystem.HealMax();
            playerData.coins -= cost;
            CoinsChanged();
            ShowTab(4);
        }
    }
    #endregion

    public void CoinsChanged()
    {
        coinText.text = cointName + " " + playerData.coins;
    }

    public void AddCoins()
    {
        playerData.coins += 100;
        CoinsChanged();
    }

    public void LoadNextLevel()
    {
        playerData.LoadNextLevel(levelNames[playerData.levelsCompleted]);
    }
}

[Serializable]
public class ShopItem<TShopItem>
{
    public string itemName;
    public TShopItem item;
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;

    [Header("Coins")]
    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] private string cointName = "Coins:" ;

    [Header("Cooldowns")]
    [SerializeField] private CoolDownVisual primaryVisual;
    [SerializeField] private CoolDownVisual secondaryVisual;
    [SerializeField] private CoolDownVisual abilityVisual;
    [SerializeField] private bool hasAbility = false;

    [Header("Item")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemCountText;
    [SerializeField] private GameObject itemVisualParent;
    private bool hasItem = false;

    public void Start()
    {
        UpdateCoins();
    }

    public void UpdateHealth(float healthPercent)
    {
        hpSlider.value = healthPercent;
    }

    public void UpdateCooldowns(float primaryCooldown, float secondaryCooldown, float abilityCooldown = 0)
    {
        primaryVisual.ChangeFill(primaryCooldown);
        secondaryVisual.ChangeFill(secondaryCooldown);
        if(hasAbility)
        {
            abilityVisual.ChangeFill(abilityCooldown);
        }
        if(hasItem)
        {
            itemVisualParent.SetActive(true);
        }
    }

    public void UpdateCoins()
    {
        coinCountText.text = cointName + " " + PlayerData.instance.coins;
    }

    public void ChangeWeapon(Sprite primarySprite, Sprite secondarySprite)
    {
        primaryVisual.image.sprite = primarySprite;
        secondaryVisual.image.sprite = secondarySprite;
    }

    public void ChangeAbility(Sprite abilitySprite, bool hasAbility = true)
    {
        this.hasAbility = hasAbility;
        if (hasAbility)
        {
            abilityVisual.image.sprite = abilitySprite;
            abilityVisual.parentObject.SetActive(true);
        }
        else
        {
            abilityVisual.parentObject.SetActive(false);
        }
    }

    public void ChangeItem(Sprite itemSprite, int amount, bool hasItem = true)
    {
        this.hasItem = hasItem;
        if (hasItem)
        {
            itemImage.sprite = itemSprite;
            itemCountText.text = amount.ToString();
        }
        else
        {
            itemVisualParent.SetActive(false);
        }
        
    }

    public void ChangeItemAmount(int amount)
    {
        itemCountText.text = amount.ToString();
    }
}

[System.Serializable]
public class CoolDownVisual
{
    public GameObject parentObject;
    public Image image;
    public Image fill;

    public void ChangeFill(float amount)
    {
        fill.fillAmount = amount;
    }
}


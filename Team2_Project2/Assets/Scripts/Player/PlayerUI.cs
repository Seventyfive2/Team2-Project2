using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    [Header("Notification")]
    [SerializeField] private TMP_Text notificationText = null;
    private Color notificationTextColor;

    private Vector3 startVector;
    private Vector3 targetVector;

    public void Start()
    {
        UpdateCoins();

        notificationTextColor = Color.black;
        notificationText.transform.parent.gameObject.SetActive(false);
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

    public void TriggerNotification(string text, float displayTime = 1.5f)
    {
        notificationText.text = text;
        notificationText.color = notificationTextColor;
        notificationText.transform.parent.gameObject.SetActive(true);

        targetVector = ColorValues(Color.clear);
        startVector = ColorValues(notificationTextColor);

        StartCoroutine(FadeText(displayTime));
    }

    IEnumerator FadeText(float fadeWait)
    {
        yield return new WaitForSeconds(fadeWait);

        //StartLerping();

        notificationText.transform.parent.gameObject.SetActive(false);
    }

    public Vector3 ColorValues(Color color)
    {
        Vector3 vector = new Vector3(1, 1, 1);

        vector.x = color.r;
        vector.y = color.g;
        vector.z = color.b;

        return vector;
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


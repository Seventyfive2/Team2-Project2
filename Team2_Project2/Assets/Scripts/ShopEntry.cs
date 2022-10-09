using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopEntry : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    public TMP_Text itemPrice;
    public Button buyButton;
    public Slider slider;
    public TMP_Text itemAmt;

    public void Initialize(Sprite sprite, string name, int price, bool useAmt = false ,bool useSlider = false)
    {
        itemImage.sprite = sprite;
        itemNameText.text = name;
        itemPrice.text = "Price: " + price;

        if(useAmt)
        {
            itemAmt.gameObject.SetActive(true);
        }

        if(useSlider)
        {
            slider.gameObject.SetActive(true);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private ShopItemData item;
    private ShopUI shopUI;

    public void Setup(ShopItemData data, ShopUI ui)
    {
        item = data;
        shopUI = ui;

        switch (item.itemType)
        {
            case ShopItemType.Weapon:
                icon.sprite = item.weaponData.icon;
                descriptionText.text = item.description;
                break;

            case ShopItemType.Potion:
                icon.sprite = item.potionData.icon;
                descriptionText.text = item.description;
                break;
        }

        priceText.text = $"–‚Î ~ {item.price}";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => shopUI.TryBuy(item));
    }
}

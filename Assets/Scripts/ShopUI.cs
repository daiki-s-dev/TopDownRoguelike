using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("商品スロット（4つ）")]
    public ShopItemSlot[] itemSlots;

    [Header("UI")]
    public TextMeshProUGUI messageText;
    public Button closeButton;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        gameObject.SetActive(false);
        closeButton.onClick.AddListener(Close);
    }

    public void Open(ShopItemData[] items, PlayerInventory inventory)
    {
        playerInventory = inventory;
        messageText.text = "";

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < items.Length)
            {
                itemSlots[i].gameObject.SetActive(true);
                itemSlots[i].Setup(items[i], this);
            }
            else
            {
                itemSlots[i].gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(true);
    }

    public void TryBuy(ShopItemData item)
    {
        // 魔石消費（今回セッション分を優先）
        bool success = PlayerCrystalInventory.Instance.ConsumeCrystal(item.price);

        if (!success)
        {
            messageText.text = "魔石が足りていません";
            return;
        }

        switch (item.itemType)
        {
            case ShopItemType.Weapon:
                playerInventory.AddWeapon(item.weaponData);
                messageText.text = item.weaponData.weaponName + " を購入しました！";
                break;

            case ShopItemType.Potion:
                AddPotionToInventory(item.potionData);
                break;
        }
    }

    private void AddPotionToInventory(PotionData potion)
    {
        switch (potion.type)
        {
            case PotionData.PotionType.HP:
                playerInventory.AddHPPotion(1);
                messageText.text = "HPポーションを購入しました！";
                break;

            case PotionData.PotionType.MP:
                playerInventory.AddMPPotion(1);
                messageText.text = "MPポーションを購入しました！";
                break;
        }
    }



    public void Close()
    {
        gameObject.SetActive(false);
    }
}

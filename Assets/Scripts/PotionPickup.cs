using UnityEngine;

// IInteractable を実装
public class PotionPickup : MonoBehaviour, IInteractable
{
    public PotionData potionData;

    // IInteractable: UI に表示される名前
    public string GetInteractName()
    {
        return potionData.itemName;
    }

    // IInteractable: インタラクト時の処理
    public void Interact(PlayerInventory inventory)
    {
        if (inventory == null) return;

        if (potionData.type == PotionData.PotionType.HP)
        {
            inventory.AddHPPotion(1);
        }
        else if (potionData.type == PotionData.PotionType.MP)
        {
            inventory.AddMPPotion(1);
        }

        Debug.Log($"{potionData.itemName} を獲得しました！");
        Destroy(gameObject);
    }
}

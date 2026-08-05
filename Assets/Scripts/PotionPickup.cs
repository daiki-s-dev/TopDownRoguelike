using UnityEngine;

/// <summary>
/// フィールド上のポーションアイテム。
/// プレイヤーが調べるとインベントリに追加され、自身は消滅する。
/// </summary>
public class PotionPickup : MonoBehaviour, IInteractable
{
    public PotionData potionData;

    /// <summary>
    /// UI に表示される名前。
    /// </summary>
    public string GetInteractName()
    {
        return potionData.itemName;
    }

    /// <summary>
    /// インタラクト時の処理。
    /// </summary>
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
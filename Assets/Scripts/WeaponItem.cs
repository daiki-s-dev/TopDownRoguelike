using UnityEngine;

/// <summary>
/// フィールド上の武器アイテム。
/// プレイヤーが調べるとインベントリに追加され、自身は消滅する。
/// </summary>
public class WeaponItem : MonoBehaviour, IInteractable
{
    public WeaponData weaponData;

    public string GetInteractName()
    {
        return weaponData != null ? weaponData.weaponName : "Weapon";
    }

    public void Interact(PlayerInventory inventory)
    {
        if (inventory != null)
            inventory.AddWeapon(weaponData);

        Destroy(gameObject);
    }
}
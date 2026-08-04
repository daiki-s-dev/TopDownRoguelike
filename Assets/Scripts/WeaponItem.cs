using UnityEngine;

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

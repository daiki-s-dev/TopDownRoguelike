using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("UŒ‚•û®")]
    public WeaponAttack swordAttack;
    public BowAttack bowAttack;

    [Header("QÆ")]
    public PlayerWeaponEquip weaponEquip;
    public PlayerAttackController attackController;

    WeaponData currentWeapon;

    public void EquipWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;

        // UŒ‚•û®Ø‚è‘Ö‚¦
        swordAttack.enabled = weapon.weaponType == WeaponType.Melee;
        bowAttack.enabled   = weapon.weaponType == WeaponType.Bow;

        // Œ©‚½–Ú‚Ì‘•”õ
        weaponEquip.EquipWeapon(weapon, attackController);

        // ‹|‚Éƒf[ƒ^‚ğ“n‚·
        if (weapon.weaponType == WeaponType.Bow)
        {
            bowAttack.weaponData = weapon;
        }
    }

    public void UnequipWeapon()
    {
        swordAttack.enabled = false;
        bowAttack.enabled = false;
        weaponEquip.UnequipWeapon();
        currentWeapon = null;
    }
}

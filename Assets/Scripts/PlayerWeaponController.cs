using UnityEngine;

/// <summary>
/// 装備中の武器タイプに応じて、剣攻撃/弓攻撃コンポーネントの有効・無効を切り替える。
/// </summary>
public class PlayerWeaponController : MonoBehaviour
{
    [Header("攻撃方式")]
    public WeaponAttack swordAttack;
    public BowAttack bowAttack;

    [Header("参照")]
    public PlayerWeaponEquip weaponEquip;
    public PlayerAttackController attackController;

    private WeaponData currentWeapon;

    public void EquipWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;

        // 攻撃方式切り替え
        swordAttack.enabled = weapon.weaponType == WeaponType.Melee;
        bowAttack.enabled = weapon.weaponType == WeaponType.Bow;

        // 見た目の装備
        weaponEquip.EquipWeapon(weapon, attackController);

        // 弓にデータを渡す
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
using UnityEngine;

/// <summary>
/// プレイヤーの武器の見た目（インスタンス生成）と、
/// 攻撃コントローラーへの装備通知を管理する。
/// </summary>
public class PlayerWeaponEquip : MonoBehaviour
{
    [Header("武器表示位置")]
    public Transform weaponPoint;

    private GameObject currentWeapon;
    private WeaponData equippedWeapon;
    public WeaponData EquippedWeapon => equippedWeapon;

    private PlayerStatus playerStatus;

    private void Awake()
    {
        playerStatus = GetComponent<PlayerStatus>();
    }

    /// <summary>
    /// 武器を装備する。
    /// </summary>
    public void EquipWeapon(WeaponData weapon, PlayerAttackController attackController)
    {
        // 既存武器破棄
        if (currentWeapon != null)
            Destroy(currentWeapon);

        equippedWeapon = weapon;

        // ステータス再計算
        playerStatus?.RecalculateStats();

        if (weapon == null)
        {
            // 武器が null の場合は AttackController も解除
            attackController?.UnequipWeapon();
            return;
        }

        currentWeapon = Instantiate(weapon.weaponPrefab, weaponPoint);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        if (weapon.weaponType == WeaponType.Staff)
            attackController?.SetWeapon(null, weapon);
        else
        {
            WeaponDamageArea area = currentWeapon.GetComponentInChildren<WeaponDamageArea>();
            attackController?.SetWeapon(area, weapon);
        }
    }

    /// <summary>
    /// 武器解除（攻撃コントローラー通知あり）。
    /// </summary>
    public void UnequipWeapon(PlayerAttackController attackController)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);

        equippedWeapon = null;

        // AttackController に解除を通知
        attackController?.UnequipWeapon();

        playerStatus?.RecalculateStats();
    }

    /// <summary>
    /// 武器解除（引数なしオーバーロード）。
    /// </summary>
    public void UnequipWeapon()
    {
        // もし攻撃コントローラーがシーン内に存在するなら取得して解除
        PlayerAttackController ac = GetComponent<PlayerAttackController>();
        UnequipWeapon(ac);
    }
}
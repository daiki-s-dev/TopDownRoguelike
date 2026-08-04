using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private WeaponData weaponData;
    private WeaponDamageArea weaponArea;

    [Header("魔法")]
    public MagicData equippedMagic;
    public Transform magicCastPoint;

    // 武器装備
    public void SetWeapon(WeaponDamageArea area, WeaponData data)
    {
        if (weaponArea != null)
            weaponArea.OnHitEnemy -= HitEnemy;

        weaponArea = area;
        weaponData = data;

        if (weaponArea != null && weaponData != null && weaponData.weaponType != WeaponType.Staff)
            weaponArea.OnHitEnemy += HitEnemy;

        Debug.Log($"[AttackController] 武器装備: {(weaponData != null ? weaponData.weaponName : "なし")}");
    }

    // 武器解除
    public void UnequipWeapon()
    {
        if (weaponArea != null)
            weaponArea.OnHitEnemy -= HitEnemy;

        weaponArea = null;
        weaponData = null; // 左クリック無効化
        Debug.Log("[AttackController] 武器解除されました");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 武器がない場合は何もしない
            if (weaponData == null)
            {
                Debug.Log("[AttackController] 武器なしで左クリック 無効化");
                return;
            }

            Debug.Log($"[AttackController] 武器ありで左クリック: {weaponData.weaponName}");

            PlayAttackSE();

            switch (weaponData.weaponType)
            {
                case WeaponType.Staff:
                    CastMagic();
                    break;
                case WeaponType.Bow:
                    // 矢発射処理
                    break;
                case WeaponType.Melee:
                    // 剣は WeaponDamageArea のイベントで判定
                    break;
            }
        }
    }

    private void HitEnemy(Collider2D enemyCollider)
    {
        if (weaponData == null) return;

        EnemyBase enemy = enemyCollider.GetComponent<EnemyBase>();
        if (enemy == null) return;

        if (weaponData.mpCost > 0 && !PlayerStatus.Instance.UseMP(weaponData.mpCost))
        {
            MPWarningUI.Instance?.Show();
            return;
        }

        bool isCritical;
        int dmg = PlayerStatus.Instance.GetWeaponDamage(weaponData, out isCritical);
        enemy.TakeDamage(dmg, transform.position, isCritical);
    }

    void CastMagic()
    {
        if (equippedMagic == null || magicCastPoint == null || weaponData == null)
            return;

        if (weaponData.mpCost > 0 && !PlayerStatus.Instance.UseMP(weaponData.mpCost))
        {
            MPWarningUI.Instance?.Show();
            return;
        }

        bool isCritical;
        int dmg = PlayerStatus.Instance.GetWeaponDamage(weaponData, out isCritical);

        GameObject obj = Instantiate(
            equippedMagic.magicPrefab,
            magicCastPoint.position,
            magicCastPoint.rotation
        );

        MagicProjectile proj = obj.GetComponent<MagicProjectile>();
        if (proj != null) proj.Init(dmg, isCritical);

        AreaMagic area = obj.GetComponent<AreaMagic>();
        if (area != null) area.Init(dmg, isCritical);
    }

    void PlayAttackSE()
    {
        if (weaponData == null || weaponData.attackSE == null) return;
        AudioManager.Instance?.PlaySE(weaponData.attackSE);
    }
}

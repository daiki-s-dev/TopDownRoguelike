using UnityEngine;

public class StaffAttack : MonoBehaviour
{
    [Header("杖設定")]
    public WeaponData weaponData;
    public MagicData magicData;
    public Transform firePoint;

    [Header("指定地点取得")]
    public LayerMask groundLayer;

    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            CastMagic();
        }
    }

    void CastMagic()
    {
        if (weaponData == null || magicData == null)
            return;

        // =========================
        // MP消費（WeaponData）
        // =========================
        if (weaponData.mpCost > 0)
        {
            if (!PlayerStatus.Instance.UseMP(weaponData.mpCost))
            {
                MPWarningUI.Instance?.ShowNotEnoughMP();
                return;
            }
        }

        bool isCritical;
        int finalDamage =
            PlayerStatus.Instance.GetWeaponDamage(weaponData, out isCritical);

        // =========================
        // 魔法タイプ分岐
        // =========================
        switch (magicData.castType)
        {
            case MagicCastType.Projectile:
                CastProjectile(finalDamage, isCritical);
                break;

            case MagicCastType.TargetArea:
                CastTargetArea(finalDamage, isCritical);
                break;
        }
    }

    // =========================
    // 飛ばす魔法
    // =========================
    void CastProjectile(int damage, bool isCritical)
    {
        if (firePoint == null) return;

        GameObject magicObj = Instantiate(
            magicData.magicPrefab,
            firePoint.position,
            firePoint.rotation
        );

        MagicProjectile proj = magicObj.GetComponent<MagicProjectile>();
        if (proj != null)
        {
            proj.Init(damage, isCritical);
        }
    }

    // =========================
    // 指定地点範囲魔法
    // =========================
    void CastTargetArea(int damage, bool isCritical)
    {
        Vector3 targetPos = GetMouseWorldPosition();

        GameObject magicObj = Instantiate(
            magicData.magicPrefab,
            targetPos,
            Quaternion.identity
        );

        AreaMagic area = magicObj.GetComponent<AreaMagic>();
        if (area != null)
        {
            area.Init(damage, isCritical);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }

    // =========================
    // マウス → 地面座標取得
    // =========================
    bool TryGetMouseGroundPosition(out Vector3 worldPos)
    {
        worldPos = Vector3.zero;

        if (mainCam == null) return false;

        Vector2 mouseWorld =
            mainCam.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(
            mouseWorld,
            Vector2.zero,
            0f,
            groundLayer
        );

        if (!hit) return false;

        worldPos = hit.point;
        worldPos.z = 0f;
        return true;
    }
}

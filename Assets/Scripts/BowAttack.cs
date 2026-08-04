using UnityEngine;

/// <summary>
/// 弓による攻撃を制御する。左クリックで矢を発射し、
/// MPを消費してダメージを算出する。
/// </summary>
public class BowAttack : MonoBehaviour
{
    [Header("弓設定")]
    public WeaponData weaponData;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 12f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireArrow();
        }
    }

    private void FireArrow()
    {
        if (weaponData == null || arrowPrefab == null || firePoint == null)
            return;

        // MP消費チェック
        if (weaponData.mpCost > 0)
        {
            if (!PlayerStatus.Instance.UseMP(weaponData.mpCost))
            {
                MPWarningUI.Instance?.ShowNotEnoughMP();
                return;
            }
        }

        bool isCritical;
        int finalDamage = PlayerStatus.Instance.GetWeaponDamage(weaponData, out isCritical);

        GameObject arrowObj = Instantiate(
            arrowPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = arrowObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * weaponData.arrowSpeed;
        }

        Arrow arrow = arrowObj.GetComponent<Arrow>();
        if (arrow != null)
        {
            arrow.Init(finalDamage, isCritical);
        }
    }
}
using UnityEngine;

public class BowAttack : MonoBehaviour
{
    [Header("弓設定")]
    public WeaponData weaponData;
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowSpeed = 12f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireArrow();
        }
    }

    void FireArrow()
    {
        if (weaponData == null || arrowPrefab == null || firePoint == null)
            return;

        // ★ MP消費チェック
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

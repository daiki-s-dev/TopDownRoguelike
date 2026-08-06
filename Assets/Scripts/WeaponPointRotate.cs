using UnityEngine;

/// <summary>
/// マウス位置に向かって武器の基準点を回転させる。
/// </summary>
public class WeaponPointRotate : MonoBehaviour
{
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ここで90度補正（必要に応じて -90 に）
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
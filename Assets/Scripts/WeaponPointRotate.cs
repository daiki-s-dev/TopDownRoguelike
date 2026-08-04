using UnityEngine;

public class WeaponPointRotate : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ← ここで90度補正（必要に応じて -90 に）
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}

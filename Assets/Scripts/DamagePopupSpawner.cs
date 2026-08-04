using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    public GameObject damagePopupCanvasPrefab;
    public float spawnOffsetY = 2f;

    public void CreatePopup(int damage, bool isCritical)
    {
        if (damagePopupCanvasPrefab == null) return;

        GameObject canvasObj = Instantiate(damagePopupCanvasPrefab);
        Vector3 spawnPos = transform.position + Vector3.up * spawnOffsetY;

        DamagePopup popup = canvasObj.GetComponentInChildren<DamagePopup>();
        if (popup != null)
            popup.Setup(damage, isCritical, spawnPos);
    }
}

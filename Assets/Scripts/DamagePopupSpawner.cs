using UnityEngine;

/// <summary>
/// 自身の位置を基準にダメージポップアップを生成するヘルパー。
/// </summary>
public class DamagePopupSpawner : MonoBehaviour
{
    [Header("設定")]
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
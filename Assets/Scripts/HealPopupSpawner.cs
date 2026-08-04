using UnityEngine;

/// <summary>
/// 自身の位置を基準に回復量ポップアップを生成するヘルパー。
/// </summary>
public class HealPopupSpawner : MonoBehaviour
{
    [Header("設定")]
    public GameObject popupPrefab;
    public float offsetY = 2f;

    public void CreatePopup(int value, HealType type)
    {
        if (popupPrefab == null) return;

        GameObject obj = Instantiate(popupPrefab);
        Vector3 pos = transform.position + Vector3.up * offsetY;

        HealPopup popup = obj.GetComponentInChildren<HealPopup>();
        if (popup != null)
            popup.Setup(value, type, pos);
    }
}
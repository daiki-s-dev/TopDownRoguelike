using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// インベントリ画面全体（開閉、スロット表示、説明文、ポーション所持数表示）を管理するシングルトン。
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    public static InventoryUIController Instance { get; private set; }

    [Header("インベントリパネル")]
    public GameObject inventoryWindow;

    [Header("スロット内のアイコン")]
    public List<UnityEngine.UI.Image> slotIcons = new List<UnityEngine.UI.Image>();

    [Header("説明文")]
    public TextMeshProUGUI descriptionText;

    [Header("常駐ポーションUI")]
    public TextMeshProUGUI hpPotionCountText;
    public TextMeshProUGUI mpPotionCountText;

    [Header("常駐ポーションUI(拡張)")]
    public TextMeshProUGUI hpPotionText;
    public TextMeshProUGUI mpPotionText;

    private bool isOpen = false;
    public bool IsOpen => isOpen;

    #region Unity Lifecycle

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        inventoryWindow?.SetActive(false);

        foreach (var icon in slotIcons) icon.enabled = false;
        if (descriptionText != null) descriptionText.text = "";
        if (hpPotionText != null) hpPotionText.text = "× 0";
        if (mpPotionText != null) mpPotionText.text = "× 0";
        if (hpPotionCountText != null) hpPotionCountText.text = "×0";
        if (mpPotionCountText != null) mpPotionCountText.text = "×0";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventory();
    }

    #endregion

    #region スロット操作

    public void AddItemToUI(WeaponData weapon)
    {
        foreach (var icon in slotIcons)
        {
            if (!icon.enabled)
            {
                icon.sprite = weapon.icon;
                icon.enabled = true;
                var slot = icon.transform.parent.GetComponent<DragSlot>();
                if (slot != null) slot.weaponData = weapon;
                return;
            }
        }
        Debug.LogWarning("[InventoryUI] インベントリが満杯です");
    }

    public void RemoveItemFromUI(WeaponData weapon)
    {
        foreach (var icon in slotIcons)
        {
            var slot = icon.transform.parent.GetComponent<DragSlot>();
            if (slot != null && slot.weaponData == weapon)
            {
                slot.weaponData = null;
                icon.sprite = null;
                icon.enabled = false;
                Debug.Log($"[InventoryUI] {weapon.weaponName} を UI から削除");
                return;
            }
        }
        Debug.LogWarning($"[InventoryUI] {weapon.weaponName} が UI に存在しません");
    }

    public void RefreshInventoryUI()
    {
        foreach (var icon in slotIcons)
        {
            var slot = icon.transform.parent.GetComponent<DragSlot>();
            if (slot != null) slot.weaponData = null;
            icon.sprite = null;
            icon.enabled = false;
        }
        ClearDescription();
        Debug.Log("[InventoryUI] UIをリフレッシュしました");
    }

    #endregion

    #region 開閉

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        if (!isOpen)
        {
            // インベントリを閉じる瞬間にドラッグを強制終了
            foreach (var slot in FindObjectsOfType<DragSlot>())
            {
                slot.CancelDrag();
            }
        }

        inventoryWindow?.SetActive(isOpen);
    }

    #endregion

    #region 説明文

    public void SetDescription(string text) => descriptionText.text = text;
    public void ClearDescription() => descriptionText.text = "";

    #endregion

    #region ポーション表示

    public void UpdatePotionUI(int hp, int mp)
    {
        if (hpPotionText != null) hpPotionText.text = $"× {hp}";
        if (mpPotionText != null) mpPotionText.text = $"× {mp}";
        if (hpPotionCountText != null) hpPotionCountText.text = $"×{hp}";
        if (mpPotionCountText != null) mpPotionCountText.text = $"×{mp}";
    }

    #endregion
}
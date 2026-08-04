using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 現在装備中の武器のアイコンと消費MPを表示するUI。
/// インベントリやポーズメニューが開いている間は非表示にする。
/// </summary>
public class EquippedWeaponUI : MonoBehaviour
{
    [Header("装備中武器参照")]
    public PlayerWeaponEquip playerWeaponEquip;

    [Header("UI Image")]
    public Image weaponIcon;

    [Header("消費MP表示")]
    public TMP_Text mpCostText;

    [Header("UI全体のパネル")]
    public GameObject panel;

    private void Update()
    {
        if (playerWeaponEquip == null || weaponIcon == null) return;

        // インベントリ or ポーズ中なら非表示
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        bool pauseOpen = PauseMenuManager.IsPaused;

        bool hideUI = inventoryOpen || pauseOpen;

        if (panel != null)
            panel.SetActive(!hideUI);

        if (hideUI) return;

        UpdateWeaponUI();
    }

    /// <summary>
    /// 装備中武器のUI更新。
    /// </summary>
    private void UpdateWeaponUI()
    {
        WeaponData weapon = playerWeaponEquip.EquippedWeapon;

        if (weapon != null)
        {
            // アイコン
            if (weapon.icon != null)
            {
                weaponIcon.sprite = weapon.icon;
                weaponIcon.enabled = true;
            }
            else
            {
                weaponIcon.enabled = false;
            }

            // 消費MP表示（0でも表示）
            if (mpCostText != null)
            {
                mpCostText.text = $"{weapon.mpCost}";
                mpCostText.gameObject.SetActive(true);
            }
        }
        else
        {
            weaponIcon.enabled = false;
            if (mpCostText != null)
                mpCostText.gameObject.SetActive(false);
        }
    }
}
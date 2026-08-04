using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Update()
    {
        if (playerWeaponEquip == null || weaponIcon == null) return;

        // -----------------------------
        // インベントリ or ポーズ中なら非表示
        // -----------------------------
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        bool pauseOpen = PauseMenuManager.IsPaused;

        bool hideUI = inventoryOpen || pauseOpen;

        if (panel != null)
            panel.SetActive(!hideUI);

        if (hideUI) return;

        // -----------------------------
        // 装備中武器のUI更新
        // -----------------------------
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

            // ★ 消費MP表示（0でも表示）
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

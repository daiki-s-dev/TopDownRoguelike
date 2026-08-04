using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 武器を装備するための専用スロット。
/// インベントリの DragSlot からドラッグ＆ドロップされたアイテムを受け取り装備する。
/// </summary>
public class EquipSlot : MonoBehaviour, IDropHandler
{
    [Header("このスロット内のアイコン")]
    public Image itemIcon;

    [Header("装備管理")]
    public PlayerWeaponEquip playerWeaponEquip;

    [Header("プレイヤー攻撃コントローラ")]
    public PlayerAttackController playerAttackController;

    [Header("スロットが保持する武器データ")]
    public WeaponData weaponData;

    public void OnDrop(PointerEventData eventData)
    {
        DragSlot dragSlot = eventData.pointerDrag?.GetComponent<DragSlot>();
        if (dragSlot == null || dragSlot.weaponData == null)
            return;

        // アイコン・データをセット
        itemIcon.sprite = dragSlot.itemIcon.sprite;
        itemIcon.enabled = true;
        weaponData = dragSlot.weaponData;

        // 武器装備（ここで attackController を渡す）
        if (playerWeaponEquip != null && playerAttackController != null)
        {
            playerWeaponEquip.EquipWeapon(weaponData, playerAttackController);
        }

        // 元スロットをクリア
        dragSlot.ClearSlot();
    }
}
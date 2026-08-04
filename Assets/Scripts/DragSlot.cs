using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum SlotCategory { Any, Weapon, Accessory }

    [Header("このスロットが受け付けるカテゴリ")]
    public SlotCategory slotCategory = SlotCategory.Any;

    [Header("このスロット内の ItemIcon")]
    public Image itemIcon;

    [Header("スロットが保持する武器データ")]
    public WeaponData weaponData;

    [Header("スロットにカーソルが載ったときの枠画像")]
    public Image highlightFrame;

    [Header("装備管理")]
    public PlayerWeaponEquip playerWeaponEquip;

    [Header("プレイヤー攻撃コントローラ")]
    public PlayerAttackController playerAttackController;

    private Image draggingIcon;
    private Transform canvas;

    void Start()
    {
        var canvasObj = GameObject.Find("Canvas");
        if (canvasObj != null) canvas = canvasObj.transform;

        if (highlightFrame != null)
            highlightFrame.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightFrame != null)
            highlightFrame.enabled = true;

        if (weaponData != null && InventoryUIController.Instance != null)
        {
            InventoryUIController.Instance.SetDescription(weaponData.description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightFrame != null)
            highlightFrame.enabled = false;

        if (InventoryUIController.Instance != null)
        {
            InventoryUIController.Instance.ClearDescription();
        }
    }

    // ★★★★★ 右クリック処理追加 ★★★★★
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (weaponData != null)
            {
                DropMenuController.Instance.OpenMenu(this, Input.mousePosition);
            }
        }
    }

    // ドラッグ開始
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (weaponData == null || itemIcon == null || !itemIcon.enabled)
            return;

        GameObject go = new GameObject("DraggingIcon");
        draggingIcon = go.AddComponent<Image>();

        if (canvas != null)
            draggingIcon.transform.SetParent(canvas, false);

        draggingIcon.sprite = itemIcon.sprite;
        draggingIcon.preserveAspect = true;
        draggingIcon.raycastTarget = false;
        draggingIcon.rectTransform.sizeDelta = itemIcon.rectTransform.sizeDelta;

        draggingIcon.transform.SetAsLastSibling(); // ★重要
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingIcon == null) return;

        RectTransform canvasRect = canvas as RectTransform;
        Vector2 localPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            null,
            out localPos
        );

        draggingIcon.rectTransform.localPosition = localPos;
    }

    // ドロップ時のカテゴリチェック
    public void OnDrop(PointerEventData eventData)
    {
        DragSlot originSlot = eventData.pointerDrag?.GetComponent<DragSlot>();
        if (originSlot == null || originSlot == this)
            return;

        WeaponData incomingData = originSlot.weaponData;

        if (!CanAccept(incomingData))
        {
            Debug.Log("このスロットは対応していないカテゴリです");
            return;
        }

        if (!originSlot.CanAccept(weaponData))
        {
            Debug.Log("入れ替え元がこのカテゴリを受け付けないため交換不可");
            return;
        }

        // 交換処理
        Sprite tempSprite = itemIcon.sprite;
        bool tempEnabled = itemIcon.enabled;
        WeaponData tempData = weaponData;

        itemIcon.sprite = originSlot.itemIcon.sprite;
        itemIcon.enabled = originSlot.itemIcon.enabled;
        weaponData = originSlot.weaponData;

        originSlot.itemIcon.sprite = tempSprite;
        originSlot.itemIcon.enabled = tempEnabled;
        originSlot.weaponData = tempData;

        UpdateWeaponEquip();
        originSlot.UpdateWeaponEquip();
    }

    public bool CanAccept(WeaponData data)
    {
        if (data == null) return true;

        switch (slotCategory)
        {
            case SlotCategory.Any:
                return true;
            case SlotCategory.Weapon:
                return data.category == ItemCategory.Weapon;
            case SlotCategory.Accessory:
                return data.category == ItemCategory.Accessory;
        }
        return false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingIcon != null)
            Destroy(draggingIcon.gameObject);
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        weaponData = null;
        UpdateWeaponEquip();
    }

    public void UpdateWeaponEquip()
    {
        if (playerWeaponEquip == null || playerAttackController == null)
            return;

        if (weaponData != null && itemIcon.enabled)
        {
            playerWeaponEquip.EquipWeapon(weaponData, playerAttackController);
        }
        else
        {
            playerWeaponEquip.UnequipWeapon();
        }
    }

    public void CancelDrag()
    {
        if (draggingIcon != null)
        {
            Destroy(draggingIcon.gameObject);
            draggingIcon = null;
        }
    }
}

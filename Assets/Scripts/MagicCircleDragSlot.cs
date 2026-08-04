using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 魔法陣の一時インベントリに表示される武器スロット。
/// ドラッグして合成スロットへ渡すことができる。
/// </summary>
public class MagicCircleDragSlot : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler,
    IDropHandler
{
    [Header("UI")]
    public Image itemIcon;
    public Image highlightFrame;

    [Header("Data")]
    public WeaponData weaponData;

    private Canvas canvas;
    private Image draggingIcon;
    private MagicCircleDescriptionUI descriptionUI;

    #region Unity Lifecycle

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        // 旧: descriptionUI = FindObjectOfType<MagicCircleDescriptionUI>(true);
        descriptionUI = FindFirstObjectByType<MagicCircleDescriptionUI>(FindObjectsInactive.Include);

        if (highlightFrame != null)
            highlightFrame.enabled = false;

        Clear();
    }

    #endregion

    #region スロット操作

    public void Setup(WeaponData data)
    {
        weaponData = data;
        itemIcon.sprite = data.icon;
        itemIcon.enabled = true;
    }

    public void Clear()
    {
        weaponData = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        if (highlightFrame != null) highlightFrame.enabled = false;
    }

    #endregion

    #region ホバー表示

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightFrame != null) highlightFrame.enabled = true;
        if (weaponData != null) descriptionUI?.Set(weaponData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightFrame != null) highlightFrame.enabled = false;
        descriptionUI?.Clear();
    }

    #endregion

    #region ドラッグ＆ドロップ

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (weaponData == null) return;

        draggingIcon = new GameObject("DraggingIcon").AddComponent<Image>();
        draggingIcon.transform.SetParent(canvas.transform, false);
        draggingIcon.sprite = itemIcon.sprite;
        draggingIcon.preserveAspect = true;
        draggingIcon.raycastTarget = false;
        draggingIcon.rectTransform.sizeDelta = itemIcon.rectTransform.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingIcon != null) draggingIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingIcon != null) Destroy(draggingIcon.gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var sourceSlot = eventData.pointerDrag?.GetComponent<MagicCircleSynthesisSlot>();
        if (sourceSlot != null && sourceSlot.weaponData != null)
        {
            weaponData = sourceSlot.weaponData;
            itemIcon.sprite = weaponData.icon;
            itemIcon.enabled = true;
            sourceSlot.Clear();
        }
    }

    #endregion
}
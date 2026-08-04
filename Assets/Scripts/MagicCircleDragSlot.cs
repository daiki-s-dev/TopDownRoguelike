using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    Canvas canvas;
    Image draggingIcon;
    MagicCircleDescriptionUI descriptionUI;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        // ‹Œ: descriptionUI = FindObjectOfType<MagicCircleDescriptionUI>(true);
        descriptionUI = FindFirstObjectByType<MagicCircleDescriptionUI>(FindObjectsInactive.Include);

        if (highlightFrame != null)
            highlightFrame.enabled = false;

        Clear();
    }


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
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MagicCircleSynthesisSlot : MonoBehaviour,
    IDropHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("UI")]
    public Image icon;
    public Image highlightFrame;

    [Header("Data")]
    public WeaponData weaponData;

    private Canvas canvas;
    private Image draggingIcon;
    private MagicCircleDescriptionUI descriptionUI;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        // ‹Œ: descriptionUI = FindObjectOfType<MagicCircleDescriptionUI>(true);
        descriptionUI = FindFirstObjectByType<MagicCircleDescriptionUI>(FindObjectsInactive.Include);

        if (highlightFrame != null)
            highlightFrame.enabled = false;

        Clear();
    }

    #region Drag & Drop
    public bool CanAccept(WeaponData incoming)
    {
        if (incoming == null) return false;
        if (weaponData != null) return weaponData.rarity == incoming.rarity;
        return true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var drag = eventData.pointerDrag?.GetComponent<MagicCircleDragSlot>();
        if (drag != null && drag.weaponData != null)
        {
            if (CanAccept(drag.weaponData))
            {
                weaponData = drag.weaponData;
                icon.sprite = weaponData.icon;
                icon.enabled = true;
                drag.Clear();
            }
            return;
        }

        var drag2 = eventData.pointerDrag?.GetComponent<MagicCircleSynthesisSlot>();
        if (drag2 != null && drag2.weaponData != null && drag2 != this)
        {
            if (CanAccept(drag2.weaponData))
            {
                weaponData = drag2.weaponData;
                icon.sprite = weaponData.icon;
                icon.enabled = true;
                drag2.Clear();
            }
        }
    }
    #endregion

    #region Hover
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

    #region Drag Visual
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (weaponData == null) return;
        draggingIcon = new GameObject("DraggingIcon").AddComponent<Image>();
        draggingIcon.transform.SetParent(canvas.transform, false);
        draggingIcon.sprite = icon.sprite;
        draggingIcon.preserveAspect = true;
        draggingIcon.raycastTarget = false;
        draggingIcon.rectTransform.sizeDelta = icon.rectTransform.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingIcon != null) draggingIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingIcon != null) Destroy(draggingIcon.gameObject);
    }
    #endregion

    public void Clear()
    {
        weaponData = null;
        icon.sprite = null;
        icon.enabled = false;
        if (highlightFrame != null) highlightFrame.enabled = false;
        Debug.Log($"[SynthesisSlot] {name} cleared");
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BlessingUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("アイコン")]
    public Image iconImage;

    [Header("Tooltip")]
    public GameObject tooltipPrefab; // Tooltip プレハブ（背景 + TMP_Text）
    private GameObject tooltipInstance;
    private TextMeshProUGUI tooltipText;

    [Header("オフセット")]
    public Vector3 tooltipOffset = new Vector3(15, 30, 0);

    private string blessingName;
    private string description;

    private bool isPointerOver = false;

    public void SetBlessing(Blessing blessing)
    {
        if (blessing == null) return;

        blessingName = blessing.blessingName;
        description = blessing.description;

        if (iconImage != null && blessing.icon != null)
            iconImage.sprite = blessing.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        HideTooltip();
    }

    private void ShowTooltip()
    {
        if (tooltipPrefab == null || tooltipInstance != null) return;

        Canvas canvas = null;
        if (BlessingManager.Instance != null && BlessingManager.Instance.panel != null)
            canvas = BlessingManager.Instance.panel.GetComponentInParent<Canvas>();

        if (canvas == null) return;

        tooltipInstance = Instantiate(tooltipPrefab, canvas.transform);
        tooltipInstance.transform.localScale = Vector3.one;
        tooltipInstance.SetActive(true);

        // CanvasGroup 設定（マウスイベントを通す）
        CanvasGroup cg = tooltipInstance.GetComponent<CanvasGroup>();
        if (cg == null) cg = tooltipInstance.AddComponent<CanvasGroup>();
        cg.alpha = 1f;
        cg.blocksRaycasts = false;
        cg.interactable = false;

        tooltipText = tooltipInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (tooltipText != null)
            tooltipText.text = $"{blessingName}\n{description}";
    }

    private void HideTooltip()
    {
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
            tooltipInstance = null;
            tooltipText = null;
        }
    }

    private void Update()
    {
        // Tooltip マウス追従
        if (tooltipInstance != null)
        {
            RectTransform rt = tooltipInstance.GetComponent<RectTransform>();
            float height = rt.rect.height;

            Vector3 mousePos = Input.mousePosition;
            tooltipInstance.transform.position =
                mousePos + new Vector3(tooltipOffset.x, tooltipOffset.y + height * 0.5f, 0);

            // Inventory や PauseMenu が開かれたら自動で消す
            bool inventoryOpen = InventoryUIController.Instance != null && InventoryUIController.Instance.IsOpen;
            bool pauseOpen = PauseMenuManager.IsPaused;
            bool blessingPanelHidden = BlessingManager.Instance != null && !BlessingManager.Instance.panel.gameObject.activeInHierarchy;

            if (!isPointerOver || inventoryOpen || pauseOpen || blessingPanelHidden)
                HideTooltip();
        }
    }
}

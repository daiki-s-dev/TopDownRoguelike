using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BlessingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;                // Inspectorでセット
    public TextMeshProUGUI blessingText; // Inspectorでセット
    public Image blessingIcon;           // Inspectorでセット

    private string description;          // 説明文
    private BlessingSelectUI ui;         // 親UI参照

    public void SetBlessing(Blessing blessing, BlessingSelectUI uiRef)
    {
        ui = uiRef;

        if (blessingText != null)
            blessingText.text = blessing.blessingName;

        if (blessingIcon != null && blessing.icon != null)
            blessingIcon.sprite = blessing.icon;

        description = blessing.description;
    }

    // カーソルが乗ったら説明文を表示
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui != null)
            ui.ShowDescription(description);
    }

    // カーソルが離れたら説明文を非表示
    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui != null)
            ui.HideDescription();
    }
}

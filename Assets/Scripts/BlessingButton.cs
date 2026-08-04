using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 祝福選択UIに並ぶ1つのボタン。
/// カーソルが乗ると親UIに説明文の表示を依頼する。
/// </summary>
public class BlessingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("参照")]
    public Button button;                // Inspectorでセット
    public TextMeshProUGUI blessingText;  // Inspectorでセット
    public Image blessingIcon;            // Inspectorでセット

    private string description;   // 説明文
    private BlessingSelectUI ui;  // 親UI参照

    public void SetBlessing(Blessing blessing, BlessingSelectUI uiRef)
    {
        ui = uiRef;

        if (blessingText != null)
            blessingText.text = blessing.blessingName;

        if (blessingIcon != null && blessing.icon != null)
            blessingIcon.sprite = blessing.icon;

        description = blessing.description;
    }

    /// <summary>
    /// カーソルが乗ったら説明文を表示。
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui != null)
            ui.ShowDescription(description);
    }

    /// <summary>
    /// カーソルが離れたら説明文を非表示。
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui != null)
            ui.HideDescription();
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 祝福を獲得した際に表示するポップアップ。
/// 一定時間表示後、自動で非表示になる。
/// </summary>
public class BlessingGetPopup : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public TextMeshProUGUI text;

    [Header("表示時間")]
    public float displayTime = 2f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Raycast を絶対にブロックしない
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        // 最初は中身だけ非表示
        SetVisible(false);
    }

    public void Show(Blessing blessing)
    {
        if (blessing == null) return;

        if (iconImage != null)
        {
            iconImage.sprite = blessing.icon;
            iconImage.enabled = true;
        }

        if (text != null)
        {
            text.text = $"{blessing.blessingName} を獲得しました";
            text.enabled = true;
        }

        SetVisible(true);

        CancelInvoke();
        Invoke(nameof(Hide), displayTime);
    }

    private void Hide()
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (iconImage != null)
            iconImage.enabled = visible;

        if (text != null)
            text.enabled = visible;
    }
}
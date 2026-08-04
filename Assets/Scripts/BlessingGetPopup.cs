using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlessingGetPopup : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public TextMeshProUGUI text;

    [Header("表示時間")]
    public float displayTime = 2f;

    private CanvasGroup canvasGroup;

    void Awake()
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

    void Hide()
    {
        SetVisible(false);
    }

    void SetVisible(bool visible)
    {
        if (iconImage != null)
            iconImage.enabled = visible;

        if (text != null)
            text.enabled = visible;
    }
}

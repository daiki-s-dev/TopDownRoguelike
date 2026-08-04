using TMPro;
using UnityEngine;

/// <summary>
/// ガチャの結果メッセージを一定時間表示するポップアップ。
/// </summary>
public class GachaPopup : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI text;

    [Header("表示時間")]
    public float displayTime = 2f;

    private void Awake()
    {
        if (text != null)
            text.gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        if (text == null) return;

        text.text = message;
        text.gameObject.SetActive(true);

        CancelInvoke();
        Invoke(nameof(Hide), displayTime);
    }

    private void Hide()
    {
        if (text != null)
            text.gameObject.SetActive(false);
    }
}
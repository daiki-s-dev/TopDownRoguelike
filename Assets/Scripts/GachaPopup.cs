using UnityEngine;
using TMPro;

public class GachaPopup : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI text;

    [Header("•\Ž¦ŽžŠÔ")]
    public float displayTime = 2f;

    void Awake()
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

    void Hide()
    {
        if (text != null)
            text.gameObject.SetActive(false);
    }
}

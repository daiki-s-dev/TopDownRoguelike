using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// インタラクト選択肢UIの1項目分の表示を担当する。
/// </summary>
public class InteractOptionUI : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Image background;

    /// <summary>
    /// 表示テキストを設定する。
    /// </summary>
    public void SetText(string s)
    {
        if (label != null) label.text = s;
    }
}
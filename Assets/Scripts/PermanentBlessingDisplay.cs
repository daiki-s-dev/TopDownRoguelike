using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 取得済み恒久祝福の一覧表示に使う1項目分のUI（アイコンと所持数）。
/// </summary>
public class PermanentBlessingDisplay : MonoBehaviour
{
    public Image icon;
    public TMP_Text countText;

    // 名前は非表示にするので nameText は不要
    public void SetData(PermanentBlessing blessing, int count)
    {
        icon.sprite = blessing.icon;
        countText.text = $"x{count}";
    }
}
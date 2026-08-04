using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

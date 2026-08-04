using TMPro;
using UnityEngine;

/// <summary>
/// 魔法陣UIで武器の説明文を表示するコンポーネント。
/// </summary>
public class MagicCircleDescriptionUI : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;

    public void Set(WeaponData data)
    {
        if (data == null) return;

        descriptionText.text = data.description;
    }

    public void Clear()
    {
        descriptionText.text = "";
    }

    private string GetRarityColor(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return "#FFFFFF";
            case ItemRarity.Uncommon: return "#66FF66";
            case ItemRarity.Rare: return "#66CCFF";
            case ItemRarity.Epic: return "#CC66FF";
            case ItemRarity.Legendary: return "#FF9933";
        }
        return "#FFFFFF";
    }
}
using UnityEngine;

/// <summary>
/// ポーション1種類分のデータ。
/// </summary>
[CreateAssetMenu(menuName = "Item/Potion")]
public class PotionData : ScriptableObject
{
    public enum PotionType { HP, MP }

    [Header("ポーション名")]
    public string itemName = "Potion";

    [Header("アイコン")]
    public Sprite icon;

    [Header("ポーションの種類")]
    public PotionType type;

    [Header("回復量")]
    public int restoreAmount = 20;

    [Header("説明文")]
    [TextArea]
    public string description = "体力または魔力を回復するポーションです。";
}
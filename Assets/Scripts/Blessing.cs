using UnityEngine;

/// <summary>
/// 祝福の種類。
/// </summary>
public enum BlessingType
{
    AttackUp,           // 攻撃力アップ
    MagicUp,            // 魔力アップ
    MaxHPUp,             // 最大HPアップ
    MaxMPUp,             // 最大MPアップ
    HPRegenUp,           // HP自動回復量アップ
    MPRegenUp,           // MP自動回復量アップ
    CriticalRateUp,      // クリティカル率アップ
    CriticalDamageUp,    // クリティカルダメージ倍率アップ
    PotionBoost,         // ポーション回復量アップ
    CristalDropRateUp,   // ドロップ率アップ
    // 追加可能
}

/// <summary>
/// 祝福1種類分のデータを保持する ScriptableObject。
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "NewBlessing", menuName = "Blessing/BlessingData")]
public class Blessing : ScriptableObject
{
    [Header("基本情報")]
    public string blessingName;
    public BlessingType type;
    public Sprite icon;

    [Header("効果")]
    public float value = 1f;          // 効果量
    public bool isMultiplier = true;  // trueなら倍率、falseなら固定値（実数）

    [Header("説明")]
    [TextArea] // Inspectorで複数行を入力可能
    public string description;
}
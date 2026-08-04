using UnityEngine;

/// <summary>
/// 魔法の発動形式。
/// </summary>
public enum MagicCastType
{
    Projectile,   // 飛ばす
    TargetArea    // 指定地点範囲
}

/// <summary>
/// 魔法1種類分のデータ。
/// </summary>
[CreateAssetMenu(menuName = "GameData/Magic")]
public class MagicData : ScriptableObject
{
    public string magicName;
    public MagicCastType castType;

    // 飛ばす魔法 / 範囲魔法 共通
    public GameObject magicPrefab;
}
using UnityEngine;

public enum MagicCastType
{
    Projectile,   // 飛ばす
    TargetArea    // 指定地点範囲
}

[CreateAssetMenu(menuName = "GameData/Magic")]
public class MagicData : ScriptableObject
{
    public string magicName;
    public MagicCastType castType;

    // 飛ばす魔法 / 範囲魔法 共通
    public GameObject magicPrefab;
}

/// <summary>
/// BGMの種類。
/// </summary>
public enum BGMType
{
    Title,
    Lobby,
    Dungeon,
    Boss
}

/// <summary>
/// SE（効果音）の種類。
/// </summary>
public enum SEType
{
    // プレイヤー / 敵
    PlayerDamage,
    EnemyDamage,

    // 攻撃
    MeleeAttack,
    BowAttack,
    MagicAttack,

    // イベント
    PortalEnter,
    MagicStoneGet,
    BlessingGet,

    // UI
    ButtonHover,
    ButtonClick,

    // アイテム
    PotionUse,
    ChestOpen
}
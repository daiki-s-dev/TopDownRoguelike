using UnityEngine;

/// <summary>
/// アイテムの大分類。
/// </summary>
public enum ItemCategory
{
    Weapon,
    Accessory,
    Consumable
}

/// <summary>
/// アイテムのレアリティ。
/// </summary>
public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

/// <summary>
/// 武器タイプ。
/// </summary>
public enum WeaponType
{
    Melee,   // 剣など近接
    Bow,     // 弓
    Staff    // 杖
}

/// <summary>
/// 武器1種類分のデータ。
/// ダメージ計算・装備補正・見た目・ドロップ情報などをまとめて保持する。
/// </summary>
[CreateAssetMenu(fileName = "WeaponData", menuName = "GameData/Weapon")]
public class WeaponData : ScriptableObject
{
    #region 基本情報

    [Header("カテゴリ")]
    public ItemCategory category = ItemCategory.Weapon;

    [Header("レアリティ")]
    public ItemRarity rarity = ItemRarity.Common;

    [Header("武器タイプ")]
    public WeaponType weaponType;

    [Header("武器名 / アイコン")]
    public string weaponName;
    public Sprite icon;

    [Header("攻撃SE（攻撃開始時）")]
    public AudioClip attackSE;

    #endregion

    #region スキル / 使用コスト

    [Header("MP消費")]
    public int mpCost = 0;

    #endregion

    #region ダメージ計算用ステータス

    [Header("基礎ダメージ")]
    public int baseDamage = 0;

    [Header("ステータススケール")]
    [Tooltip("プレイヤー攻撃力の参照率（0.5 = 50%）")]
    public float attackScale = 0f;

    [Tooltip("プレイヤー魔力の参照率（0.5 = 50%）")]
    public float magicScale = 0f;

    #endregion

    #region 攻撃速度・プレハブ

    [Header("攻撃速度")]
    public float attackRate = 1f;

    [Header("武器プレハブ")]
    public GameObject weaponPrefab;

    #endregion

    #region 弓専用設定

    [Header("弓専用")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f;

    #endregion

    #region 装備時ステータス補正

    [Header("装備時ステータス補正（加算）")]
    public int bonusAttack = 0;
    public int bonusMaxHP = 0;
    public int bonusMaxMP = 0;
    public int bonusMagic = 0;

    [Range(0f, 1f)]
    public float bonusCriticalRate = 0f;

    public float bonusCriticalDamage = 0f;

    #endregion

    #region ドロップ / UI

    [Header("ドロップ時の拾えるプレハブ")]
    public GameObject dropPrefab;

    [Header("説明文")]
    [TextArea(2, 4)]
    public string description;

    #endregion
}
using UnityEngine;

/// <summary>
/// ショップで扱う商品の種類。
/// </summary>
public enum ShopItemType
{
    Weapon,
    Potion
}

/// <summary>
/// ショップの商品1種類分のデータ。
/// </summary>
[CreateAssetMenu(menuName = "Shop/ShopItemData")]
public class ShopItemData : ScriptableObject
{
    public ShopItemType itemType;

    [Header("共通")]
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public int price;

    [Header("武器")]
    public WeaponData weaponData;

    [Header("ポーション")]
    public PotionData potionData;
}
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ShopItemData")]
public class ShopItemData : ScriptableObject
{
    public ShopItemType itemType;

    [Header("‹¤’Ê")]
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public int price;

    [Header("•Ší")]
    public WeaponData weaponData;

    [Header("ƒ|[ƒVƒ‡ƒ“")]
    public PotionData potionData;
}

public enum ShopItemType
{
    Weapon,
    Potion
}

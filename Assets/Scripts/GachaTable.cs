using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public enum GachaItemType
{
    Weapon,
    Accessory,
    Potion,
    MagicStone
}

// アンコモン追加 + NoDropも扱う
[System.Serializable]
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class GachaItem
{
    public string itemName;
    public GachaItemType itemType;
    public Rarity rarity;
    public GameObject prefab;  // 実際に取得したときに生成する場合
}

[System.Serializable]
public class MagicStoneDrop
{
    public int amount;                 // 何個出るか
    [Range(0f, 1f)] public float probability; // 魔石カテゴリ内の確率
}

[CreateAssetMenu(fileName = "GachaTable", menuName = "Gacha/GachaTable")]
public class GachaTable : ScriptableObject
{
    [Header("アイテム設定")]
    public List<GachaItem> weapons = new List<GachaItem>();
    public List<GachaItem> accessories = new List<GachaItem>();
    public List<GachaItem> potions = new List<GachaItem>();

    [Header("大カテゴリ確率（合計1以下推奨）")]
    [Range(0f, 1f)] public float noDropRate = 0.5f;      // 何も出ない
    [Range(0f, 1f)] public float magicStoneRate = 0.25f; // 魔石カテゴリ
    [Range(0f, 1f)] public float itemRate = 0.25f;       // アイテムカテゴリ

    [Header("魔石ドロップ設定（魔石カテゴリ内で正規化される）")]
    public List<MagicStoneDrop> magicStoneDrops = new List<MagicStoneDrop>();

    [Header("アイテムレアリティ確率（アイテムカテゴリ内で正規化される）")]
    [Range(0f, 1f)] public float commonRate = 0.5f;
    [Range(0f, 1f)] public float uncommonRate = 0.2f;
    [Range(0f, 1f)] public float rareRate = 0.2f;
    [Range(0f, 1f)] public float epicRate = 0.08f;
    [Range(0f, 1f)] public float legendaryRate = 0.02f;

    /// <summary>
    /// ガチャを回してアイテムまたは魔石を取得
    /// </summary>
    public object Roll()
    {
        float roll = Random.value;

        // 大カテゴリ判定
        if (roll <= noDropRate)
        {
            Debug.Log("[GachaTable] 何もドロップしません");
            return null;
        }
        else if (roll <= noDropRate + magicStoneRate)
        {
            // 魔石カテゴリ
            int stones = RollMagicStone();
            Debug.Log($"[GachaTable] 魔石ドロップ: {stones}");
            return stones; // intで返す
        }
        else
        {
            // アイテムカテゴリ
            GachaItem item = RollItem();
            if (item != null)
                Debug.Log($"[GachaTable] アイテムドロップ: {item.itemName} ({item.rarity})");
            else
                Debug.Log("[GachaTable] アイテムカテゴリなのにアイテムが見つからなかった");
            return item;
        }
    }

    /// <summary>
    /// 魔石ドロップを決定
    /// </summary>
    public int RollMagicStone()
    {
        if (magicStoneDrops.Count == 0) return 0;

        float totalProb = 0f;
        foreach (var drop in magicStoneDrops) totalProb += drop.probability;
        if (totalProb <= 0f) return 0;

        float roll = Random.value;
        float cumulative = 0f;
        foreach (var drop in magicStoneDrops)
        {
            cumulative += drop.probability / totalProb; // 正規化
            if (roll <= cumulative) return drop.amount;
        }

        return 0;
    }

    /// <summary>
    /// アイテムドロップを決定
    /// </summary>
    public GachaItem RollItem()
    {
        // レアリティ決定
        float totalRate = commonRate + uncommonRate + rareRate + epicRate + legendaryRate;
        if (totalRate <= 0f) return null;

        float roll = Random.value * totalRate; // 正規化
        Rarity rarity;
        if (roll <= legendaryRate) rarity = Rarity.Legendary;
        else if (roll <= legendaryRate + epicRate) rarity = Rarity.Epic;
        else if (roll <= legendaryRate + epicRate + rareRate) rarity = Rarity.Rare;
        else if (roll <= legendaryRate + epicRate + rareRate + uncommonRate) rarity = Rarity.Uncommon;
        else rarity = Rarity.Common;

        // プール作成
        List<GachaItem> pool = new List<GachaItem>();
        pool.AddRange(weapons.FindAll(i => i.rarity == rarity));
        pool.AddRange(accessories.FindAll(i => i.rarity == rarity));
        pool.AddRange(potions.FindAll(i => i.rarity == rarity));

        if (pool.Count == 0) return null;

        int index = Random.Range(0, pool.Count);
        return pool[index];
    }
}

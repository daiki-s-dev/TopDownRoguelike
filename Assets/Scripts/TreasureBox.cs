using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    // =========================
    // enum 定義
    // =========================

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum RareDropLevel
    {
        Lv0, Lv1, Lv2, Lv3, Lv4
    }

    // =========================
    // レアリティ別武器枠
    // =========================

    [System.Serializable]
    public class WeaponRaritySlot
    {
        public Rarity rarity;

        [Tooltip("このレアリティ枠から排出される武器")]
        public List<GameObject> weaponPrefabs;
    }

    // =========================
    // Inspector 設定
    // =========================

    [Header("武器排出設定")]
    public List<WeaponRaritySlot> weaponSlots;

    [Header("レアリティ排出率（基準値）")]
    public float commonRate = 50;
    public float uncommonRate = 30;
    public float rareRate = 15;
    public float epicRate = 4;
    public float legendaryRate = 1;

    [Header("レアドロップ強化レベル")]
    public RareDropLevel rareDropLevel = RareDropLevel.Lv0;

    [Header("排出位置")]
    public Transform dropPoint;

    // =========================
    // 外部から呼ぶ
    // =========================

    public void Open()
    {
        Rarity rarity = DrawRarity();
        GameObject weapon = DrawWeaponFromSlot(rarity);

        if (weapon == null)
        {
            Debug.LogWarning($"[{rarity}] に武器が設定されていません");
            return;
        }

        Instantiate(weapon, dropPoint.position, Quaternion.identity);
        Debug.Log($"武器排出: {weapon.name} [{rarity}]");
    }

    // =========================
    // 抽選処理
    // =========================

    Rarity DrawRarity()
    {
        Dictionary<Rarity, float> rates = new()
        {
            { Rarity.Common, commonRate },
            { Rarity.Uncommon, uncommonRate },
            { Rarity.Rare, rareRate },
            { Rarity.Epic, epicRate },
            { Rarity.Legendary, legendaryRate }
        };

        ApplyRareDropModifier(rates);

        float total = 0;
        foreach (var r in rates.Values)
            total += r;

        float rand = Random.value * total;
        float current = 0;

        foreach (var pair in rates)
        {
            current += pair.Value;
            if (rand <= current)
                return pair.Key;
        }

        return Rarity.Common;
    }

    GameObject DrawWeaponFromSlot(Rarity rarity)
    {
        var slot = weaponSlots.Find(s => s.rarity == rarity);
        if (slot == null || slot.weaponPrefabs.Count == 0)
            return null;

        return slot.weaponPrefabs[
            Random.Range(0, slot.weaponPrefabs.Count)];
    }

    // =========================
    // レアドロ率補正
    // =========================

    void ApplyRareDropModifier(Dictionary<Rarity, float> rates)
    {
        float multiplier = rareDropLevel switch
        {
            RareDropLevel.Lv0 => 1.0f,
            RareDropLevel.Lv1 => 1.1f,
            RareDropLevel.Lv2 => 1.25f,
            RareDropLevel.Lv3 => 1.5f,
            RareDropLevel.Lv4 => 2.0f,
            _ => 1.0f
        };

        // Rare 以上だけ補正
        rates[Rarity.Rare]       *= multiplier;
        rates[Rarity.Epic]       *= multiplier;
        rates[Rarity.Legendary]  *= multiplier;

        // Common を少し下げる（任意）
        rates[Rarity.Common] *= 1f / multiplier;
    }
}

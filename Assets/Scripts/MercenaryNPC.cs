using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 傭兵NPCのロジック。話しかけるとレアリティ抽選を行い、武器を1つ排出する。
/// </summary>
public class MercenaryNPC : MonoBehaviour
{
    #region enum定義（宝箱と共通）

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

    #endregion

    #region レアリティ別武器枠

    [System.Serializable]
    public class WeaponRaritySlot
    {
        public Rarity rarity;
        public List<GameObject> weaponPrefabs;
    }

    #endregion

    [Header("武器排出設定")]
    public List<WeaponRaritySlot> weaponSlots;

    [Header("レアリティ排出率")]
    public float commonRate = 50;
    public float uncommonRate = 30;
    public float rareRate = 15;
    public float epicRate = 4;
    public float legendaryRate = 1;

    [Header("レアドロップ補正")]
    public RareDropLevel rareDropLevel = RareDropLevel.Lv0;

    [Header("武器排出位置")]
    public Transform dropPoint;

    [Header("一度きり設定")]
    public bool onlyOnce = true;

    private bool hasGivenWeapon = false;

    #region 外部から呼ぶ（会話）

    public void Talk()
    {
        if (onlyOnce && hasGivenWeapon)
        {
            Debug.Log("傭兵「もう渡せる武器はないぞ」");
            return;
        }

        GiveRandomWeapon();
        hasGivenWeapon = true;
    }

    #endregion

    #region 武器排出

    private void GiveRandomWeapon()
    {
        Rarity rarity = DrawRarity();
        GameObject weapon = DrawWeaponFromSlot(rarity);

        if (weapon == null)
        {
            Debug.LogWarning($"[{rarity}] に武器が設定されていません");
            return;
        }

        Instantiate(weapon, dropPoint.position, Quaternion.identity);
        Debug.Log($"傭兵から武器獲得: {weapon.name} [{rarity}]");
    }

    #endregion

    #region 抽選処理（宝箱と同じ）

    private Rarity DrawRarity()
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

    private GameObject DrawWeaponFromSlot(Rarity rarity)
    {
        var slot = weaponSlots.Find(s => s.rarity == rarity);
        if (slot == null || slot.weaponPrefabs.Count == 0)
            return null;

        return slot.weaponPrefabs[
            Random.Range(0, slot.weaponPrefabs.Count)];
    }

    #endregion

    #region レア率補正

    private void ApplyRareDropModifier(Dictionary<Rarity, float> rates)
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

        rates[Rarity.Rare]      *= multiplier;
        rates[Rarity.Epic]      *= multiplier;
        rates[Rarity.Legendary] *= multiplier;
        rates[Rarity.Common]    *= 1f / multiplier;
    }

    #endregion
}
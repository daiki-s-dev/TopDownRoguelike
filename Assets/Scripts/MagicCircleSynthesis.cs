using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔法陣での武器合成ロジック。
/// 同レアリティの武器2つを消費し、1段階上のレアリティの武器を排出する。
/// </summary>
public class MagicCircleSynthesis : MonoBehaviour
{
    [Header("合成スロット")]
    public MagicCircleSynthesisSlot slotA;
    public MagicCircleSynthesisSlot slotB;

    [Header("ドロップ設定")]
    public Transform spawnPoint;
    public List<RarityDropTable> dropTables;

    [Header("UI")]
    public MagicCircleUIController uiController;

    // PlayerInventory はコードで取得
    private PlayerInventory playerInventory;

    private void Awake()
    {
        // シーン内の PlayerInventory を取得
        playerInventory = FindFirstObjectByType<PlayerInventory>();

        if (playerInventory == null)
        {
            Debug.LogWarning("[MagicCircleSynthesis] シーン内に PlayerInventory が見つかりません");
        }
    }

    /// <summary>
    /// 魔法陣で合成を試みる。
    /// </summary>
    public SynthesisResult TrySynthesize()
    {
        if (slotA.weaponData == null || slotB.weaponData == null)
            return SynthesisResult.EmptySlot;

        if (slotA.weaponData.rarity != slotB.weaponData.rarity)
            return SynthesisResult.DifferentRarity;

        if (slotA.weaponData.rarity == ItemRarity.Legendary)
            return SynthesisResult.CannotSynthesize;

        GameObject resultPrefab = GetRandomDrop(slotA.weaponData.rarity + 1);
        if (resultPrefab == null)
            return SynthesisResult.CannotSynthesize;

        // --- インベントリ削除 ---
        if (playerInventory != null)
        {
            Debug.Log("[MagicCircle] 武器削除開始");
            playerInventory.RemoveWeapon(slotA.weaponData);
            playerInventory.RemoveWeapon(slotB.weaponData);
        }
        else
        {
            Debug.LogWarning("[MagicCircle] PlayerInventory が取得できていません");
        }

        // --- 結果ドロップ ---
        if (spawnPoint != null)
            Instantiate(resultPrefab, spawnPoint.position, Quaternion.identity);
        else
            Debug.LogWarning("[MagicCircle] spawnPoint が設定されていません");

        // スロットリセット
        ResetSlots();

        // UIを閉じる
        uiController?.Close();

        return SynthesisResult.Success;
    }

    /// <summary>
    /// スロットをクリアする。
    /// </summary>
    public void ResetSlots()
    {
        slotA?.Clear();
        slotB?.Clear();
    }

    /// <summary>
    /// 指定レアリティのランダムドロップを取得する。
    /// </summary>
    private GameObject GetRandomDrop(ItemRarity rarity)
    {
        var table = dropTables.Find(t => t.resultRarity == rarity);
        if (table == null || table.dropPrefabs.Count == 0)
            return null;

        return table.dropPrefabs[Random.Range(0, table.dropPrefabs.Count)];
    }
}
using UnityEngine;

public class GachaPoolInteract : MonoBehaviour, IInteractable
{
    [Header("ガチャ設定")]
    public GachaTable gachaTable;

    [Header("取得演出")]
    public GachaPopup popup; // 取得テキストを表示するUI

    public int requiredCrystals = 1; // ガチャ1回に必要な魔石

    [Header("UI表示用")]
    public string interactName = "魔法の池";

    [Header("アイテムドロップポイント")]
    public Transform dropPoint; // ここにアイテムを生成

    // -----------------------
    // ■ IInteractable 実装
    // -----------------------
    public string GetInteractName() => interactName;

    public void Interact(PlayerInventory inv)
    {
        if (gachaTable == null || popup == null) return;

        var crystalInventory = PlayerCrystalInventory.Instance;
        if (crystalInventory == null)
        {
            Debug.LogWarning("PlayerCrystalInventory が存在しません");
            return;
        }

        // 魔石消費
        bool canUse = crystalInventory.ConsumeCrystal(requiredCrystals);
        if (!canUse)
        {
            popup.Show("魔石が足りません！");
            return;
        }

        // ガチャを回す
        object result = gachaTable.Roll();

        // 結果の判定
        if (result == null)
        {
            popup.Show("何も出ませんでした…");
        }
        else if (result is int stones)
        {
            crystalInventory.AddCrystal(stones);
            popup.Show($"{stones} 個の魔石を獲得しました！");
        }
        else if (result is GachaItem item)
        {
            popup.Show($"{item.itemName} ({item.rarity}) を獲得しました！");

            // DropPoint にプレハブを生成
            if (item.prefab != null && dropPoint != null)
            {
                Instantiate(item.prefab, dropPoint.position, Quaternion.identity);
            }
            else if (item.prefab == null)
            {
                Debug.LogWarning("取得したアイテムの prefab が設定されていません: " + item.itemName);
            }
            else if (dropPoint == null)
            {
                Debug.LogWarning("DropPoint が設定されていません");
            }
        }

        Debug.Log("ガチャ回転完了");
    }
}

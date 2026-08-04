using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("武器管理")]
    public List<WeaponData> weapons = new List<WeaponData>();
    public InventoryUIController uiController;
    public Transform player;

    [Header("ポーション所持数")]
    public int hpPotionCount = 0;
    public int mpPotionCount = 0;

    // 武器追加
    public void AddWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        weapons.Add(weapon);
        Debug.Log($"[PlayerInventory] 武器取得: {weapon.weaponName}");

        uiController?.AddItemToUI(weapon);
    }

    // 武器削除（魔法陣合成用）
    public void RemoveWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        WeaponData found = weapons.Find(w => w.weaponName == weapon.weaponName && w.rarity == weapon.rarity);
        if (found != null)
        {
            weapons.Remove(found);
            Debug.Log($"[PlayerInventory] 武器 {found.weaponName} をインベントリから削除");

            uiController?.RemoveItemFromUI(found);
        }
        else
        {
            Debug.LogWarning($"[PlayerInventory] 削除対象の武器が見つかりません: {weapon.weaponName}");
        }
    }

    // 武器ドロップ
    public void DropWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        RemoveWeapon(weapon);

        if (weapon.dropPrefab != null)
        {
            Vector3 dropPos = player.position + player.forward * 0.7f;
            GameObject obj = Instantiate(weapon.dropPrefab, dropPos, Quaternion.identity);
            WeaponItem wi = obj.GetComponent<WeaponItem>();
            if (wi != null) wi.weaponData = weapon;

            Debug.Log($"[PlayerInventory] 武器 {weapon.weaponName} をドロップしました");
        }
        else
        {
            Debug.LogWarning($"[PlayerInventory] {weapon.weaponName} の dropPrefab が設定されていません");
        }
    }

    // ポーション追加・使用
    public void AddHPPotion(int amount = 1)
    {
        hpPotionCount += amount;
        Debug.Log($"[PlayerInventory] HPポーション +{amount}（合計：{hpPotionCount}）");
        uiController?.UpdatePotionUI(hpPotionCount, mpPotionCount);
    }

    public void AddMPPotion(int amount = 1)
    {
        mpPotionCount += amount;
        Debug.Log($"[PlayerInventory] MPポーション +{amount}（合計：{mpPotionCount}）");
        uiController?.UpdatePotionUI(hpPotionCount, mpPotionCount);
    }

    public bool UseHPPotion()
    {
        if (hpPotionCount <= 0) return false;
        hpPotionCount--;
        uiController?.UpdatePotionUI(hpPotionCount, mpPotionCount);
        return true;
    }

    public bool UseMPPotion()
    {
        if (mpPotionCount <= 0) return false;
        mpPotionCount--;
        uiController?.UpdatePotionUI(hpPotionCount, mpPotionCount);
        return true;
    }

    public void ClearInventory()
    {
        weapons.Clear();
        hpPotionCount = 0;
        mpPotionCount = 0;
        uiController?.RefreshInventoryUI();
        uiController?.UpdatePotionUI(0, 0);
        Debug.Log("[PlayerInventory] インベントリをクリアしました");
    }
}

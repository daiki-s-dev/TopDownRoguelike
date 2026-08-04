using UnityEngine;

public class PotionItem : MonoBehaviour
{
    [Header("使用するポーション (ScriptableObject)")]
    public PotionData hpPotion;
    public PotionData mpPotion;

    [Header("デバッグ用キー (任意)")]
    public KeyCode hpKey = KeyCode.Alpha1;
    public KeyCode mpKey = KeyCode.Alpha2;

    void Update()
    {
        if (PlayerStatus.Instance == null) return;

        // HPポーション使用（1キー）
        if (hpPotion != null && Input.GetKeyDown(hpKey))
        {
            UseHPPotion();
        }

        // MPポーション使用（2キー）
        if (mpPotion != null && Input.GetKeyDown(mpKey))
        {
            UseMPPotion();
        }
    }

    //===============================
    // HPポーション
    //===============================
    public void UseHPPotion()
    {
        var inv = PlayerStatus.Instance.GetComponent<PlayerInventory>();
        if (inv == null || inv.hpPotionCount <= 0)
        {
            Debug.Log("HPポーションがありません！");
            return;
        }

        // ● HP を回復
        bool ok = PlayerStatus.Instance.UsePotion(
            Mathf.RoundToInt(hpPotion.restoreAmount * PlayerStatus.Instance.GetMultiplier(BlessingType.PotionBoost))
        );

        if (ok)
        {
            // ● インベントリ側のストックを減らす
            inv.hpPotionCount--;

            // ● UI 更新！！！ ← 今まで足りなかった部分
            InventoryUIController.Instance.UpdatePotionUI(inv.hpPotionCount, inv.mpPotionCount);

            Debug.Log($"{hpPotion.itemName} を使用 → HP回復 / 残り：{inv.hpPotionCount}");
        }
    }

    //===============================
    // MPポーション
    //===============================
    public void UseMPPotion()
    {
        var inv = PlayerStatus.Instance.GetComponent<PlayerInventory>();
        if (inv == null || inv.mpPotionCount <= 0)
        {
            Debug.Log("MPポーションがありません！");
            return;
        }

        // ● MP を回復
        bool ok = PlayerStatus.Instance.UsePotionMP(
            Mathf.RoundToInt(mpPotion.restoreAmount * PlayerStatus.Instance.GetMultiplier(BlessingType.PotionBoost))
        );

        if (ok)
        {
            // ● インベントリ側のストックを減らす
            inv.mpPotionCount--;

            // ● UI 更新！！！ ← 超重要
            InventoryUIController.Instance.UpdatePotionUI(inv.hpPotionCount, inv.mpPotionCount);

            Debug.Log($"{mpPotion.itemName} を使用 → MP回復 / 残り：{inv.mpPotionCount}");
        }
    }
}

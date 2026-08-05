using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ショップとのインタラクション。
/// 生成時に候補からランダムに商品を確定し、以降は同じ品揃えを表示する。
/// </summary>
public class ShopInteract : MonoBehaviour, IInteractable
{
    [Header("インタラクト表示名")]
    public string shopName = "ショップ";

    [Header("ショップ設定（候補）")]
    public ShopItemData[] shopItems;   // 全候補
    public ShopUI shopUI;

    [Header("表示数")]
    public int displayItemCount = 4;

    private bool canInteract;
    private PlayerInventory playerInventory;

    // このショップ専用の確定商品リスト
    private ShopItemData[] fixedItems;

    private void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();

        // ショップ生成時に一度だけ抽選
        DecideShopItems();
    }

    private void Update()
    {
        if (!canInteract) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenShop();
        }
    }

    #region IInteractable

    public string GetInteractName()
    {
        return shopName;
    }

    public void Interact(PlayerInventory inventory)
    {
        OpenShop();
    }

    #endregion

    #region 内部処理

    private void OpenShop()
    {
        if (shopUI == null || playerInventory == null) return;

        // 確定済みアイテムを渡す
        shopUI.Open(fixedItems, playerInventory);
    }

    /// <summary>
    /// ランダム抽選を一度だけ行う。
    /// </summary>
    private void DecideShopItems()
    {
        if (shopItems == null || shopItems.Length == 0)
        {
            Debug.LogWarning("ショップ候補アイテムが設定されていません");
            fixedItems = new ShopItemData[0];
            return;
        }

        List<ShopItemData> pool = new List<ShopItemData>(shopItems);
        List<ShopItemData> result = new List<ShopItemData>();

        int count = Mathf.Min(displayItemCount, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index); // 重複防止
        }

        fixedItems = result.ToArray();
    }

    #endregion

    #region Trigger 判定

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        canInteract = false;
        shopUI.Close();
    }

    #endregion
}
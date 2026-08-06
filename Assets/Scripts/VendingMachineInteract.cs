using UnityEngine;

/// <summary>
/// 自動販売機とのインタラクション。
/// 魔石を消費してHP/MPポーションをランダムに1つ排出する。
/// </summary>
public class VendingMachineInteract : MonoBehaviour, IInteractable
{
    [Header("コスト")]
    public int cost = 100;

    [Header("排出アイテム（Prefab）")]
    public GameObject hpPotionPrefab;
    public GameObject mpPotionPrefab;

    [Header("説明UI（プレハブ内）")]
    public GameObject hintUI;
    public GameObject lackMoneyUI;

    private bool isPlayerInside = false;

    private void Start()
    {
        if (hintUI != null) hintUI.SetActive(false);
        if (lackMoneyUI != null) lackMoneyUI.SetActive(false);
    }

    public string GetInteractName()
    {
        return "自動販売機";
    }

    public void Interact(PlayerInventory inventory)
    {
        if (!PlayerCrystalInventory.Instance.ConsumeCrystal(cost))
        {
            ShowLackMoney();
            return;
        }

        GameObject rewardPrefab = DrawPotion();

        Instantiate(
            rewardPrefab,
            transform.position + Vector3.down * 0.5f,
            Quaternion.identity
        );

        // いったんエラーメッセージは消す
        if (lackMoneyUI != null)
            lackMoneyUI.SetActive(false);

        // プレイヤーがまだ範囲内なら Hint を再表示
        if (isPlayerInside && hintUI != null)
            hintUI.SetActive(true);

        PlayBuyEffect();
    }

    private GameObject DrawPotion()
    {
        return Random.value < 0.5f ? hpPotionPrefab : mpPotionPrefab;
    }

    private void ShowLackMoney()
    {
        if (lackMoneyUI != null)
            lackMoneyUI.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = true;

        if (hintUI != null)
            hintUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = false;

        if (hintUI != null)
            hintUI.SetActive(false);
        if (lackMoneyUI != null)
            lackMoneyUI.SetActive(false);
    }

    private void PlayBuyEffect()
    {
        // SE / エフェクト
    }
}
using UnityEngine;

/// <summary>
/// 傭兵NPCとのインタラクション。
/// onlyOnce が true の場合、一度話すと再度話しかけられなくなる。
/// </summary>
public class MercenaryInteract : MonoBehaviour, IInteractable
{
    [Header("傭兵ロジック")]
    public MercenaryNPC mercenaryNPC;

    [Header("設定")]
    [Tooltip("trueなら一度だけ武器をくれる")]
    public bool onlyOnce = true;

    private bool hasGivenWeapon = false;

    /// <summary>
    /// UIに表示される名前。
    /// </summary>
    public string GetInteractName()
    {
        if (onlyOnce && hasGivenWeapon)
            return "……（もう何もくれなさそうだ）";

        return "傭兵に話しかける";
    }

    /// <summary>
    /// PlayerInteractor から呼ばれる。
    /// </summary>
    public void Interact(PlayerInventory inventory)
    {
        if (onlyOnce && hasGivenWeapon)
            return;

        // 傭兵ロジック実行
        mercenaryNPC.Talk();

        hasGivenWeapon = true;

        if (onlyOnce)
            DisableInteraction();
    }

    private void DisableInteraction()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }
}
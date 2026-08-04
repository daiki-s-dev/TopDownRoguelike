using UnityEngine;

public class MercenaryInteract : MonoBehaviour, IInteractable
{
    [Header("傭兵ロジック")]
    public MercenaryNPC mercenaryNPC;

    [Header("設定")]
    [Tooltip("trueなら一度だけ武器をくれる")]
    public bool onlyOnce = true;

    private bool hasGivenWeapon = false;

    // UIに表示される名前
    public string GetInteractName()
    {
        if (onlyOnce && hasGivenWeapon)
            return "……（もう何もくれなさそうだ）";

        return "傭兵に話しかける";
    }

    // PlayerInteractor から呼ばれる
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

    void DisableInteraction()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }
}

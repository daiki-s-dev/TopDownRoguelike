using UnityEngine;

public class BlessingStatueInteract : MonoBehaviour, IInteractable
{
    [Header("神像ロジック")]
    public BlessingStatue blessingStatue;

    [Header("設定")]
    public bool onlyOnce = true;

    private bool hasPrayed = false;

    public string GetInteractName()
    {
        if (onlyOnce && hasPrayed)
            return "……（もう祈れない）";

        return "祈る";
    }

    public void Interact(PlayerInventory inventory)
    {
        if (onlyOnce && hasPrayed)
            return;

        PlayerStatus playerStatus = inventory.GetComponent<PlayerStatus>();
        if (playerStatus == null)
        {
            Debug.LogWarning("BlessingStatueInteract: PlayerStatus が見つかりません");
            return;
        }

        blessingStatue.Pray(playerStatus);

        hasPrayed = true;

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

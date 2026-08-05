using UnityEngine;

/// <summary>
/// 恒久祝福購入UIを開く神像とのインタラクション。
/// プレイヤーが範囲から離れると自動でUIを閉じる。
/// </summary>
public class ShrineInteract : MonoBehaviour, IInteractable
{
    [Header("恒久祝福UI")]
    public PermanentBlessingUIController uiController;

    private bool playerInRange = false;

    private void Update()
    {
        // プレイヤーが離れた場合に自動でUIを閉じる
        if (!playerInRange && uiController.uiRoot.activeSelf)
        {
            uiController.CloseUI();
        }
    }

    public void Interact(PlayerInventory inventory)
    {
        if (uiController != null && playerInRange)
        {
            uiController.OpenUI();
        }
    }

    public string GetInteractName() => "神像";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
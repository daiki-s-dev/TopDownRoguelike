using UnityEngine;

public class ShrineInteract : MonoBehaviour, IInteractable
{
    [Header("çPãvèjïüUI")]
    public PermanentBlessingUIController uiController;

    private bool playerInRange = false;

    void Update()
    {
        // ÉvÉåÉCÉÑÅ[Ç™ó£ÇÍÇΩèÍçáÇ…é©ìÆÇ≈UIÇï¬Ç∂ÇÈ
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

    public string GetInteractName() => "ê_ëú";

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

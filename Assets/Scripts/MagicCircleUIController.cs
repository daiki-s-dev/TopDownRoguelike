using UnityEngine;

public class MagicCircleUIController : MonoBehaviour
{
    private MagicCircleInventoryUI inventoryUI;
    private MagicCircleDescriptionUI descriptionUI;
    private PlayerInventory playerInventory;

    private MagicCircleInteract currentInteract;
    public MagicCircleSynthesis synthesis;

    void Awake()
    {
        inventoryUI = GetComponentInChildren<MagicCircleInventoryUI>(true);
        descriptionUI = GetComponentInChildren<MagicCircleDescriptionUI>(true);

        if (synthesis == null)
            synthesis = GetComponentInChildren<MagicCircleSynthesis>(true);

        playerInventory = FindFirstObjectByType<PlayerInventory>();

        gameObject.SetActive(false);
    }

    public void Open(MagicCircleInteract interact)
    {
        currentInteract = interact;
        gameObject.SetActive(true);

        if (synthesis == null)
        {
            synthesis = GetComponentInChildren<MagicCircleSynthesis>(true);
            if (synthesis == null)
            {
                Debug.LogError("MagicCircleSynthesis が見つかりません！");
                return;
            }
        }

        synthesis.ResetSlots(); // ★ 開くたびに必ずリセット

        var tempInventory = new MagicCircleTempInventory(playerInventory);
        inventoryUI.Build(tempInventory);
        descriptionUI.Clear();
    }

    public void Close()
    {
        descriptionUI.Clear();

        if (synthesis != null)
            synthesis.ResetSlots();
        else
            Debug.LogWarning("Synthesis が null です");

        gameObject.SetActive(false);

        if (currentInteract != null)
        {
            currentInteract.OnUIClose();
            currentInteract = null;
        }
    }

    public void Commit(WeaponData a, WeaponData b)
    {
        playerInventory.weapons.Remove(a);
        playerInventory.weapons.Remove(b);
    }
}

using UnityEngine;

public class MagicCircleInventoryUI : MonoBehaviour
{
    public Transform content;
    public MagicCircleDragSlot slotPrefab;

    MagicCircleTempInventory inventory;

    public void Build(MagicCircleTempInventory inv)
    {
        inventory = inv;

        foreach (Transform c in content)
            Destroy(c.gameObject);

        foreach (var weapon in inventory.weapons)
        {
            var slot = Instantiate(slotPrefab, content);
            slot.Setup(weapon);
        }
    }
}

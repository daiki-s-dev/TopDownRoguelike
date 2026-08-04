using UnityEngine;

/// <summary>
/// 魔法陣UI内に表示される一時インベントリのスロット一覧を構築する。
/// </summary>
public class MagicCircleInventoryUI : MonoBehaviour
{
    public Transform content;
    public MagicCircleDragSlot slotPrefab;

    private MagicCircleTempInventory inventory;

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
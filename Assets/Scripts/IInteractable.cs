using UnityEngine;

/// <summary>
/// プレイヤーが調べる／話しかけるなどの操作を行える対象を表すインターフェース。
/// </summary>
public interface IInteractable
{
    string GetInteractName();
    void Interact(PlayerInventory inventory);
}
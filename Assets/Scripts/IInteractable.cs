using UnityEngine;

public interface IInteractable
{
    string GetInteractName();
    void Interact(PlayerInventory inventory);
}


// Assets/Scripts/PlayerInteract.cs
using UnityEngine;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    public PlayerInventory inventory;
    public InteractionUIController uiController;

    private List<IInteractable> nearbyObjects = new List<IInteractable>();

    void Update()
    {
        // E キーで現在選択中の対象を実行
        if (nearbyObjects.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            string selectedName = uiController.GetSelectedName();
            if (selectedName == null) return;

            // 名前で探す（同名が複数あるなら距離で優先するなど拡張可）
            IInteractable target = nearbyObjects.Find(obj => obj.GetInteractName() == selectedName);
            target?.Interact(inventory);
            
            // 実行後 UI 再更新（例：拾ったらリストから消える）
            RefreshUI();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interact))
        {
            if (nearbyObjects.Contains(interact)) return;
            nearbyObjects.Add(interact);
            RefreshUI();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interact))
        {
            nearbyObjects.Remove(interact);
            RefreshUI();
        }
    }

    void RefreshUI()
    {
        if (nearbyObjects.Count == 0)
        {
            uiController.Hide();
            return;
        }

        // 名前リストを作る（距離順にしたい場合はここでソート）
        var names = new List<string>();
        foreach (var obj in nearbyObjects) names.Add(obj.GetInteractName());

        uiController.ShowOptions(names);
    }
}



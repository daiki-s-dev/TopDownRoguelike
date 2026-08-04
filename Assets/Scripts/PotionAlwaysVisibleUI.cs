using UnityEngine;

public class PotionAlwaysVisibleUI : MonoBehaviour
{
    [Header("UI全体のパネル")]
    public GameObject panel;

    void Update()
    {
        if (panel == null) return;

        // インベントリが開かれているか
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        // ポーズメニューが開かれているか
        bool pauseOpen = PauseMenuManager.IsPaused;

        // インベントリまたはポーズ画面が開いていたら非表示
        bool hideUI = inventoryOpen || pauseOpen;

        panel.SetActive(!hideUI);
    }
}

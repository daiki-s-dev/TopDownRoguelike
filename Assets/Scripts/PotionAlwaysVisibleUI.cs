using UnityEngine;

/// <summary>
/// 常時表示されるポーション所持数UIパネルの表示制御。
/// インベントリやポーズメニューが開いている間は非表示にする。
/// </summary>
public class PotionAlwaysVisibleUI : MonoBehaviour
{
    [Header("UI全体のパネル")]
    public GameObject panel;

    private void Update()
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
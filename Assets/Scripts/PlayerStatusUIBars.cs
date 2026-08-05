using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのHP/MPバーと魔石所持数を表示するUI。
/// インベントリやポーズメニューが開いている間は非表示にする。
/// </summary>
public class PlayerStatusUIBars : MonoBehaviour
{
    [Header("バー画像")]
    public Image hpFill;
    public Image mpFill;

    [Header("数値テキスト")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;

    [Header("今回のセッション魔石")]
    public TextMeshProUGUI currentSessionCrystalText;

    [Header("累積魔石")]
    public TextMeshProUGUI totalCrystalText;

    [Header("バー全体のパネル")]
    public GameObject barsPanel;

    private PlayerStatus status;

    private void Start()
    {
        status = PlayerStatus.Instance;
    }

    private void Update()
    {
        if (status == null) return;

        // インベントリ or ポーズ中なら非表示
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        bool pauseOpen = PauseMenuManager.IsPaused;

        bool hideUI = inventoryOpen || pauseOpen;

        barsPanel.SetActive(!hideUI);

        if (hideUI) return;

        // HPバー更新
        hpFill.fillAmount = (float)status.currentHP / status.maxHP;

        // MPバー更新
        mpFill.fillAmount = (float)status.currentMP / status.maxMP;

        // HPテキスト更新
        hpText.text = $"{status.currentHP} / {status.maxHP}";

        // MPテキスト更新
        mpText.text = $"{status.currentMP} / {status.maxMP}";

        // クリスタル数更新
        if (PlayerCrystalInventory.Instance != null)
        {
            int currentCrystals = PlayerCrystalInventory.Instance.GetCurrentSessionCrystals();
            int totalCrystals = PlayerCrystalInventory.Instance.TotalCrystals;

            if (currentSessionCrystalText != null)
                currentSessionCrystalText.text = $"今回の魔石: {currentCrystals}";

            if (totalCrystalText != null)
                totalCrystalText.text = $"累計魔石: {totalCrystals}";
        }
    }
}
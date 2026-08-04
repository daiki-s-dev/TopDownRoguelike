using UnityEngine;
using TMPro;

public class ResultUIController : MonoBehaviour
{
    [Header("表示テキスト")]
    public TextMeshProUGUI crystalText;   // 今回獲得した魔石
    public TextMeshProUGUI timeText;      // クリアタイム

    private void Start()
    {
        // --- 魔石 ---
        int crystals = PlayerCrystalInventory.Instance.GetCurrentSessionCrystals();
        crystalText.text = $"{crystals} 個";

        // --- タイム ---
        float clearTime = TimeManager.Instance.GetElapsedTime();
        timeText.text = FormatTime(clearTime);

        // クリアしたので魔石を確定して TotalCrystals に加算
        PlayerCrystalInventory.Instance.DepositCrystals();
    }

    // 時間を 00:00.00 の形式に整える
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        float seconds = time % 60f;

        return $"{minutes:00}:{seconds:00.00}";
    }
}

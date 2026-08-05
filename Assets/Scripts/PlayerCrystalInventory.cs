using UnityEngine;

/// <summary>
/// プレイヤーが所持する魔石（クリスタル）を管理するシングルトン。
/// 今回のダンジョンで獲得した分と、持ち帰り済みの累計分を分けて管理する。
/// </summary>
public class PlayerCrystalInventory : MonoBehaviour
{
    public static PlayerCrystalInventory Instance;

    // 持ち帰った魔石（累積）
    public int TotalCrystals { get; private set; } = 0;

    // 今回のダンジョンで獲得した魔石（未確定）
    private int currentSessionCrystals = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("PlayerCrystalInventory は既に存在します。重複破棄。");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ダンジョン中に魔石を獲得する。
    /// </summary>
    public void AddCrystal(int amount)
    {
        currentSessionCrystals += amount;
        AudioManager.Instance?.PlaySE(SEType.MagicStoneGet);
        Debug.Log($"今回のセッション魔石: {currentSessionCrystals}");
    }

    /// <summary>
    /// ダンジョンクリア時に魔石を持ち帰る。
    /// </summary>
    public void DepositCrystals()
    {
        TotalCrystals += currentSessionCrystals;
        Debug.Log($"クリア！累計魔石: {TotalCrystals}");
        currentSessionCrystals = 0;
    }

    /// <summary>
    /// 死亡や途中退出時に今回の魔石を失う。
    /// </summary>
    public void ResetCurrentSession()
    {
        Debug.Log($"魔石を失った: {currentSessionCrystals}");
        currentSessionCrystals = 0;
    }

    /// <summary>
    /// 現在のセッションの魔石だけを確認したい場合。
    /// </summary>
    public int GetCurrentSessionCrystals() => currentSessionCrystals;

    /// <summary>
    /// 魔石を消費する（セッション魔石を優先して消費）。
    /// </summary>
    public bool ConsumeCrystal(int amount)
    {
        int totalAvailable = currentSessionCrystals + TotalCrystals;

        // 足りない場合
        if (totalAvailable < amount)
        {
            Debug.Log("魔石が足りません");
            return false;
        }

        // まずセッション分から消費
        if (currentSessionCrystals >= amount)
        {
            currentSessionCrystals -= amount;
        }
        else
        {
            int remaining = amount - currentSessionCrystals;
            currentSessionCrystals = 0;
            TotalCrystals -= remaining;
        }

        Debug.Log($"魔石消費: {amount} / 残り（今回:{currentSessionCrystals}, 累計:{TotalCrystals})");
        return true;
    }
}
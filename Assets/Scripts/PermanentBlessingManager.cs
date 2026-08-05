using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが取得済みの恒久祝福を管理するシングルトン。
/// セーブデータ相当の永続的な強化を保持する。
/// </summary>
public class PermanentBlessingManager : MonoBehaviour
{
    public static PermanentBlessingManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class PermanentBlessingData
    {
        public PermanentBlessing blessing;
        public int count = 0;
    }

    // 購入可能な祝福リスト
    public List<PermanentBlessing> availableBlessings = new List<PermanentBlessing>();

    // プレイヤーが取得済みの恒久祝福
    public List<PermanentBlessingData> permanentBlessings = new List<PermanentBlessingData>();

    /// <summary>
    /// 恒久祝福を追加する。
    /// </summary>
    public void AddBlessing(PermanentBlessing blessing)
    {
        var existing = permanentBlessings.Find(b => b.blessing.type == blessing.type);
        if (existing != null)
        {
            existing.count++;
        }
        else
        {
            permanentBlessings.Add(new PermanentBlessingData { blessing = blessing, count = 1 });
        }

        // PlayerStatus の再計算のみ（activeBlessingsには追加しない）
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.RecalculateStats();
        }
    }

    public int GetBlessingCount(BlessingType type)
    {
        var data = permanentBlessings.Find(b => b.blessing.type == type);
        return data != null ? data.count : 0;
    }
}
using System.Collections.Generic;
using UnityEngine;

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

    // w“ü‰Â”\‚Èj•ŸƒŠƒXƒg
    public List<PermanentBlessing> availableBlessings = new List<PermanentBlessing>();

    // ƒvƒŒƒCƒ„[‚ªæ“¾Ï‚İ‚ÌP‹vj•Ÿ
    public List<PermanentBlessingData> permanentBlessings = new List<PermanentBlessingData>();

    // P‹vj•Ÿ‚ğ’Ç‰Á
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

        // PlayerStatus ‚ÌÄŒvZ‚Ì‚İiactiveBlessings‚É‚Í’Ç‰Á‚µ‚È‚¢j
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

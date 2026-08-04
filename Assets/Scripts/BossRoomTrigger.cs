using UnityEngine;

/// <summary>
/// プレイヤーがボス部屋の入り口に触れたことを検知し、
/// BossRoomManager にボス部屋開始を通知するトリガー。
/// </summary>
public class BossRoomTrigger : MonoBehaviour
{
    [Header("Bossルーム管理")]
    public BossRoomManager bossRoomManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (bossRoomManager != null)
        {
            bossRoomManager.OnPlayerEnterRoom(); // ボス部屋開始
            gameObject.SetActive(false);         // 一度だけ
        }
        else
        {
            Debug.LogWarning("BossRoomManager がアタッチされていません！");
        }
    }
}
using UnityEngine;

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

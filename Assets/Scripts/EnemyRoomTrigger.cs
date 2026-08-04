using UnityEngine;

/// <summary>
/// プレイヤーが敵部屋の入り口に触れたことを検知し、
/// EnemyRoomManager に部屋開始を通知するトリガー。
/// </summary>
public class EnemyRoomTrigger : MonoBehaviour
{
    [Header("部屋管理")]
    public EnemyRoomManager roomManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (roomManager != null)
        {
            roomManager.OnPlayerEnterRoom(); // 部屋開始
            gameObject.SetActive(false);     // トリガーは一度だけ
        }
        else
        {
            Debug.LogWarning("EnemyRoomManager がアタッチされていません！");
        }
    }
}
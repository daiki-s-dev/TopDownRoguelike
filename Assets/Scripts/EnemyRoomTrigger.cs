using UnityEngine;

public class EnemyRoomTrigger : MonoBehaviour
{
    public EnemyRoomManager roomManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (roomManager != null)
            {
                roomManager.OnPlayerEnterRoom(); // ★ここで呼ぶ
                gameObject.SetActive(false);     // トリガーは一度だけ
            }
            else
            {
                Debug.LogWarning("EnemyRoomManager がアタッチされていません！");
            }
        }
    }
}

using UnityEngine;

/// <summary>
/// ボスフロア入場時にプレイヤーをスポーン地点へ配置するコントローラー。
/// </summary>
public class BossFloorController : MonoBehaviour
{
    [Header("プレイヤースポーン")]
    public Transform playerSpawnPoint;

    private void Start()
    {
        SpawnPlayer();
    }

    #region プレイヤー出現

    private void SpawnPlayer()
    {
        if (playerSpawnPoint == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player が見つかりません（Tag確認）");
            return;
        }

        player.transform.position = playerSpawnPoint.position;
        player.transform.rotation = playerSpawnPoint.rotation;
    }

    #endregion
}
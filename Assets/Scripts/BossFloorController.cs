using UnityEngine;

public class BossFloorController : MonoBehaviour
{
    [Header("ボス設定")]
    public GameObject[] bossPrefabs;
    public Transform bossSpawnPoint;

    [Header("プレイヤースポーン")]
    public Transform playerSpawnPoint;

    [Header("クリアポータル")]
    public GameObject clearPortalPrefab;
    public Transform clearPortalSpawnPoint;

    private GameObject currentBoss;
    private GameObject clearPortalInstance;

    void Start()
    {
        SpawnPlayer();
        SpawnBoss();
        SpawnClearPortal();
    }

    // =========================
    // プレイヤー出現
    // =========================
    void SpawnPlayer()
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

    // =========================
    // ボス生成
    // =========================
    void SpawnBoss()
    {
        if (bossPrefabs == null || bossPrefabs.Length == 0) return;
        if (bossSpawnPoint == null) return;

        GameObject prefab =
            bossPrefabs[Random.Range(0, bossPrefabs.Length)];

        currentBoss = Instantiate(
            prefab,
            bossSpawnPoint.position,
            Quaternion.identity
        );
    }

    // =========================
    // クリアポータル生成（最初から）
    // =========================
    void SpawnClearPortal()
    {
        if (clearPortalPrefab == null || clearPortalSpawnPoint == null) return;

        clearPortalInstance = Instantiate(
            clearPortalPrefab,
            clearPortalSpawnPoint.position,
            Quaternion.identity
        );
    }
}

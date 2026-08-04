using UnityEngine;

/// <summary>
/// ボスフロア入場時にプレイヤー・ボス・クリアポータルを配置するコントローラー。
/// </summary>
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

    private void Start()
    {
        SpawnPlayer();
        SpawnBoss();
        SpawnClearPortal();
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

    #region ボス生成

    private void SpawnBoss()
    {
        if (bossPrefabs == null || bossPrefabs.Length == 0) return;
        if (bossSpawnPoint == null) return;

        GameObject prefab = bossPrefabs[Random.Range(0, bossPrefabs.Length)];

        currentBoss = Instantiate(
            prefab,
            bossSpawnPoint.position,
            Quaternion.identity
        );
    }

    #endregion

    #region クリアポータル生成（最初から）

    private void SpawnClearPortal()
    {
        if (clearPortalPrefab == null || clearPortalSpawnPoint == null) return;

        clearPortalInstance = Instantiate(
            clearPortalPrefab,
            clearPortalSpawnPoint.position,
            Quaternion.identity
        );
    }

    #endregion
}
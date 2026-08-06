using System.Collections;
using UnityEngine;

/// <summary>
/// ボス部屋で出現するボス候補の一覧。
/// </summary>
[System.Serializable]
public class BossWave
{
    public GameObject[] bossPrefabs; // 出現するボス候補
}

/// <summary>
/// ボス部屋の進行を管理する。
/// プレイヤー侵入時にゲート（結界）を張り、ボスを生成。
/// ボス撃破でゲートを解除する。
/// </summary>
public class BossRoomManager : MonoBehaviour
{
    [Header("Boss設定")]
    public BossWave bossWave;

    [Header("Boss Spawn Point")]
    public Transform bossSpawnPoint;

    [Header("GateWallプレハブ")]
    public GameObject gateWallPrefab;

    private GameObject currentBoss;
    private int aliveBossCount = 0;
    private bool playerInRoom = false;

    private GameObject gateWallInstance;
    private Animator[] gateAnims;

    #region プレイヤー侵入

    public void OnPlayerEnterRoom()
    {
        if (playerInRoom) return;
        playerInRoom = true;

        SpawnGateWall();
        SpawnBoss();
    }

    #endregion

    #region GateWall生成

    private void SpawnGateWall()
    {
        if (gateWallPrefab == null) return;

        gateWallInstance = Instantiate(
            gateWallPrefab,
            transform.position,
            Quaternion.identity
        );

        gateAnims = gateWallInstance.GetComponentsInChildren<Animator>();

        foreach (var a in gateAnims)
            a.SetTrigger("Rise");
    }

    #endregion

    #region ボス生成

    private void SpawnBoss()
    {
        if (bossWave == null || bossWave.bossPrefabs.Length == 0)
        {
            Debug.LogWarning($"{name}: BossWaveが設定されていません");
            return;
        }

        GameObject prefab =
            bossWave.bossPrefabs[Random.Range(0, bossWave.bossPrefabs.Length)];

        Vector3 spawnPos =
            bossSpawnPoint != null ? bossSpawnPoint.position : transform.position;

        currentBoss = Instantiate(prefab, spawnPos, Quaternion.identity);
        currentBoss.transform.SetParent(transform);

        EnemyBase enemyBase = currentBoss.GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            aliveBossCount = 1;
            enemyBase.onEnemyDead += OnBossDead;
        }
    }

    #endregion

    #region ボス死亡時

    private void OnBossDead(EnemyBase boss)
    {
        aliveBossCount--;
        boss.onEnemyDead -= OnBossDead;

        if (aliveBossCount <= 0 && playerInRoom)
        {
            StartCoroutine(FallAndDestroyGateWall());
            OnBossRoomCleared();
        }
    }

    #endregion

    #region GateWall解除

    private IEnumerator FallAndDestroyGateWall()
    {
        if (gateWallInstance != null && gateAnims != null)
        {
            foreach (var a in gateAnims)
                a.SetTrigger("Fall");

            float length = gateAnims[0]
                .GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(length);

            Destroy(gateWallInstance);
            gateWallInstance = null;
        }
    }

    #endregion

    #region ボス部屋クリア時

    private void OnBossRoomCleared()
    {
        Debug.Log("BossRoom Cleared!");

        // ここに
        // ・BGM切り替え
        // ・UI表示
        // ・Portal有効化
        // ・GameManager通知
        // などを追加できる
    }

    #endregion
}
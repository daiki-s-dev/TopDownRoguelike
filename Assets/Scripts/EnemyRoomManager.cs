using System.Collections;
using UnityEngine;

/// <summary>
/// 1つの敵Waveの構成データ（出現する敵の種類とスポーンポイントごとの数）。
/// </summary>
[System.Serializable]
public class EnemyWave
{
    public GameObject[] enemyPrefabs;  // このWaveで出す敵の種類
    public int spawnCountPerPoint = 1; // スポーン数
}

/// <summary>
/// 通常の敵部屋を管理する。
/// プレイヤー侵入でゲートを張り、Waveを順番に消化しながら敵を出現させる。
/// 全Wave撃破でゲートを解除する。
/// </summary>
public class EnemyRoomManager : MonoBehaviour
{
    [Header("Wave設定")]
    public EnemyWave[] waves;

    [Header("GateWallプレハブ")]
    public GameObject gateWallPrefab;

    private EnemySpawnPoint[] spawnPoints;
    private int aliveEnemies = 0;
    private bool playerInRoom = false;
    private int currentWaveIndex = 0;

    private GameObject gateWallInstance;
    private Animator[] gateAnims;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
        if (spawnPoints.Length == 0)
            Debug.LogWarning($"{name}: SpawnPointがありません");
    }

    #region プレイヤー侵入

    /// <summary>
    /// プレイヤーが部屋に入った時に呼ばれる。
    /// </summary>
    public void OnPlayerEnterRoom()
    {
        if (playerInRoom) return;
        playerInRoom = true;

        SpawnGateWall();              // 壁を生成してRiseアニメ
        StartWave(currentWaveIndex);  // 最初のWaveを開始
    }

    #endregion

    #region GateWall生成

    private void SpawnGateWall()
    {
        if (gateWallPrefab == null) return;

        Vector3 spawnPos = transform.position; // 必要に応じて調整
        gateWallInstance = Instantiate(gateWallPrefab, spawnPos, Quaternion.identity);

        // 子のAnimatorをすべて取得
        gateAnims = gateWallInstance.GetComponentsInChildren<Animator>();

        // Rise Triggerを一斉に送る
        foreach (var a in gateAnims)
            a.SetTrigger("Rise");
    }

    #endregion

    #region Wave進行

    private void StartWave(int waveIndex)
    {
        if (waveIndex >= waves.Length) return;

        EnemyWave wave = waves[waveIndex];

        // 安全確認
        if (wave.enemyPrefabs == null || wave.enemyPrefabs.Length == 0)
        {
            Debug.LogWarning($"Wave {waveIndex} の enemyPrefabs が設定されていません");
            return;
        }

        foreach (var sp in spawnPoints)
        {
            for (int i = 0; i < wave.spawnCountPerPoint; i++)
            {
                GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
                GameObject enemyGO = Instantiate(prefab, sp.transform.position, Quaternion.identity);
                enemyGO.transform.SetParent(transform);

                EnemyBase enemyBase = enemyGO.GetComponent<EnemyBase>();
                if (enemyBase != null)
                {
                    aliveEnemies++;
                    enemyBase.onEnemyDead += OnEnemyDead;
                }
            }
        }
    }

    private void OnEnemyDead(EnemyBase enemy)
    {
        aliveEnemies--;
        enemy.onEnemyDead -= OnEnemyDead;

        if (aliveEnemies <= 0 && playerInRoom)
        {
            currentWaveIndex++;
            if (currentWaveIndex < waves.Length)
            {
                StartWave(currentWaveIndex);
            }
            else
            {
                // 全Waveクリア時にGateWallをFallさせて消す
                StartCoroutine(FallAndDestroyGateWall());
            }
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

            // 最初のAnimatorの長さを取得して待機
            float length = gateAnims[0].GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(length);

            Destroy(gateWallInstance);
            gateWallInstance = null;
        }
    }

    #endregion
}
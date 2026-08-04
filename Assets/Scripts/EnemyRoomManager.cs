using System.Collections;
using UnityEngine;

[System.Serializable] // Inspectorで編集可能にする
public class EnemyWave
{
    public GameObject[] enemyPrefabs;  // このWaveで出す敵の種類
    public int spawnCountPerPoint = 1; // スポーン数
}

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

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<EnemySpawnPoint>();
        if (spawnPoints.Length == 0)
            Debug.LogWarning($"{name}: SpawnPointがありません");
    }

    // プレイヤーが部屋に入った時
    public void OnPlayerEnterRoom()
    {
        if (playerInRoom) return;
        playerInRoom = true;

        SpawnGateWall();         // 壁を生成してRiseアニメ
        StartWave(currentWaveIndex); // 最初のWaveを開始
    }

    // GateWall生成とRiseアニメ
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

    // Wave開始
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

    // 敵死亡時
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

    // GateWallのFallアニメ再生と削除
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
}

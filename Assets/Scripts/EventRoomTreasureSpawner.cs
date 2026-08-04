// Assets/Scripts/EventRoomTreasureSpawner.cs
using UnityEngine;
using System.Collections.Generic;

public class EventRoomTreasureSpawner : MonoBehaviour
{
    [Header("宝箱プレハブ")]
    public GameObject normalTreasureBoxPrefab;
    public GameObject rareTreasureBoxPrefab;

    [Header("出現確率（%）")]
    [Range(0, 100)] public float normalRate = 80f;
    [Range(0, 100)] public float rareRate = 20f;

    [Header("SpawnPoint 設定")]
    public string spawnPointTag = "SpawnPoint";

    void Start()
    {
        SpawnTreasures();
    }

    void SpawnTreasures()
    {
        var spawnPoints = GetSpawnPoints();
        foreach (var point in spawnPoints)
        {
            GameObject prefab = DrawTreasurePrefab();
            if (prefab == null) continue;

            Instantiate(prefab, point.position, Quaternion.identity, transform);
        }
    }

    List<Transform> GetSpawnPoints()
    {
        List<Transform> points = new();

        foreach (Transform child in transform)
        {
            if (child.CompareTag(spawnPointTag))
                points.Add(child);
        }

        return points;
    }

    GameObject DrawTreasurePrefab()
    {
        float total = normalRate + rareRate;
        if (total <= 0) return null;

        float rand = Random.value * total;

        if (rand < normalRate)
            return normalTreasureBoxPrefab;
        else
            return rareTreasureBoxPrefab;
    }
}

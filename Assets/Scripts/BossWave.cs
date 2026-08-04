using UnityEngine;

/// <summary>
/// ボス部屋で出現するボス候補の一覧。
/// </summary>
[System.Serializable]
public class BossWave
{
    public GameObject[] bossPrefabs; // 出現するボス候補
}
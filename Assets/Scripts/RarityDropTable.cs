using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定レアリティで排出されるプレハブの一覧（魔法陣合成の結果テーブルなどに使用）。
/// </summary>
[System.Serializable]
public class RarityDropTable
{
    public ItemRarity resultRarity;

    [Header("このレアリティで排出されるプレハブ")]
    public List<GameObject> dropPrefabs = new List<GameObject>();
}
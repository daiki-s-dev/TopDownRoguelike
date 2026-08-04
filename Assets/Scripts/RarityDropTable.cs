using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RarityDropTable
{
    public ItemRarity resultRarity;

    [Header("このレアリティで排出されるプレハブ")]
    public List<GameObject> dropPrefabs = new List<GameObject>();
}

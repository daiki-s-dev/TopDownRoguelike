using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 神像本体のロジック。祈ることで祝福をランダムに1つ付与する。
/// </summary>
public class BlessingStatue : MonoBehaviour
{
    [Header("排出される祝福一覧")]
    public List<Blessing> blessings;

    [Header("獲得演出UI")]
    public BlessingGetPopup blessingGetPopup;

    public void Pray(PlayerStatus playerStatus)
    {
        if (playerStatus == null) return;

        if (blessings == null || blessings.Count == 0)
        {
            Debug.LogWarning("BlessingStatue: 祝福が設定されていません");
            return;
        }

        // ランダム抽選
        Blessing blessing = blessings[Random.Range(0, blessings.Count)];

        // プレイヤーに付与
        playerStatus.ApplyBlessing(blessing);

        // 獲得演出
        if (blessingGetPopup != null)
            blessingGetPopup.Show(blessing);

        Debug.Log($"神像の祝福獲得: {blessing.blessingName}");
    }
}
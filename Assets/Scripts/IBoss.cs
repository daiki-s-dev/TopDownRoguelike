using UnityEngine;

/// <summary>
/// ボス撃破時のコールバックを提供するインターフェース。
/// </summary>
public interface IBoss
{
    void OnBossDefeated();
}
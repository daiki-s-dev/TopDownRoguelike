using System;
using UnityEngine;

/// <summary>
/// 近接武器の当たり判定エリア。
/// 指定タグに触れると OnHitEnemy イベントを発火する。
/// </summary>
public class WeaponDamageArea : MonoBehaviour
{
    [Header("攻撃対象となるタグ")]
    public string targetTag = "Enemy";

    public Action<Collider2D> OnHitEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
            OnHitEnemy?.Invoke(other);
    }
}
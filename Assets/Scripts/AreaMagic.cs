using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔法による範囲攻撃。StaffAttack から初期化され、
/// 範囲内に入った敵1体につき1回だけダメージを与える。
/// </summary>
public class AreaMagic : MonoBehaviour
{
    [Header("寿命")]
    public float lifeTime = 0.5f; // 演出が終わるまでの時間

    private int damage;
    private bool isCritical;

    private readonly HashSet<EnemyBase> hitEnemies = new HashSet<EnemyBase>();
    private Collider2D col;

    #region Unity Lifecycle

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    #endregion

    #region 初期化

    /// <summary>
    /// StaffAttack から呼ばれる初期化。
    /// </summary>
    public void Init(int dmg, bool critical)
    {
        damage = dmg;
        isCritical = critical;

        // 念のため開始時は無効 → すぐ有効
        col.enabled = false;
        Invoke(nameof(EnableCollider), 0.05f);

        Destroy(gameObject, lifeTime);
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }

    #endregion

    #region 当たり判定

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy == null) return;

        // すでに当たった敵なら無視
        if (hitEnemies.Contains(enemy)) return;

        hitEnemies.Add(enemy);
        enemy.TakeDamage(damage, transform.position, isCritical);
    }

    #endregion
}
using UnityEngine;
using System.Collections.Generic;

public class AreaMagic : MonoBehaviour
{
    [Header("寿命")]
    public float lifeTime = 0.5f;   // 演出が終わるまでの時間

    private int damage;
    private bool isCritical;

    private HashSet<EnemyBase> hitEnemies = new HashSet<EnemyBase>();
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    /// <summary>
    /// StaffAttack から呼ばれる初期化
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

    void EnableCollider()
    {
        col.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy == null) return;

        // ★ すでに当たった敵なら無視
        if (hitEnemies.Contains(enemy)) return;

        hitEnemies.Add(enemy);
        enemy.TakeDamage(damage, transform.position, isCritical);
    }
}

using System;
using System.Collections;
using UnityEngine;

public abstract class BossBase : MonoBehaviour
{
    public Action<BossBase> onBossDead;
    public Action<int, int> onHpChanged;

    [Header("基本設定")]
    public float speed = 1.5f;
    public float attackRange = 2.5f;
    public float chaseRange = 7f;
    public float attackCooldown = 3f;
    public int maxHp = 50;
    public int hp = 50;

    [Header("攻撃設定")]
    public float chargeTime = 1f;

    [Header("ノックバック")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    protected Transform player;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected bool isAttacking = false;
    protected bool isCharging = false;
    protected bool isKnockedBack = false;
    protected bool attackLocked = false;

    protected float attackTimer = 0f;

    // =========================
    // 初期化
    // =========================
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        hp = maxHp;
    }

    // =========================
    // Update（完全ボス専用）
    // =========================
    protected virtual void Update()
    {
        if (player == null || isKnockedBack)
            return;

        attackTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (isAttacking || isCharging)
            return;

        if (distance <= attackRange && attackTimer <= 0f)
        {
            StartCoroutine(AttackSequence());
        }
        else if (distance <= chaseRange)
        {
            MoveTowardsPlayer();
        }
    }

    // =========================
    // 攻撃シーケンス（唯一の入口）
    // =========================
    private IEnumerator AttackSequence()
    {
        if (attackLocked)
            yield break;

        attackLocked = true;
        isAttacking = true;
        isCharging = true;
        attackTimer = attackCooldown;

        Debug.Log("[Boss] 攻撃開始");

        // 攻撃中は完全停止
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        yield return DoAttack();

        if (rb != null)
            rb.simulated = true;

        isAttacking = false;
        isCharging = false;
        attackLocked = false;

        Debug.Log("[Boss] 攻撃終了 → クールダウン");
    }

    // =========================
    // 移動
    // =========================
    protected virtual void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        UpdateAnimation(dir);
    }

    // =========================
    // ダメージ処理
    // =========================
    public virtual void TakeDamage(int damage, Vector2 hitSourcePosition)
    {
        if (hp <= 0) return;

        hp -= damage;
        onHpChanged?.Invoke(hp, maxHp);

        Vector2 knockDir = (transform.position - (Vector3)hitSourcePosition).normalized;
        StartCoroutine(KnockbackRoutine(knockDir));

        if (hp <= 0)
            Die();
    }

    protected virtual IEnumerator KnockbackRoutine(Vector2 dir)
    {
        if (rb == null) yield break;

        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    protected virtual void Die()
    {
        Debug.Log("[Boss] 撃破");
        onBossDead?.Invoke(this);
        Destroy(gameObject);
    }

    // =========================
    // アニメ
    // =========================
    protected virtual void UpdateAnimation(Vector2 dir) { }

    // =========================
    // ★ ボス専用攻撃本体
    // =========================
    protected abstract IEnumerator DoAttack();
}

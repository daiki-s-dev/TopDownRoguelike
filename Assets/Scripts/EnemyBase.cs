using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public Action<EnemyBase> onEnemyDead;   // 死亡通知イベント
    public Action<int, int> onHpChanged;    // HP変更通知イベント

    [Header("基本設定")]
    public float speed = 2f;
    public float attackRange = 1.5f;
    public float stopDistance = 2f;
    public float attackCooldown = 2f;
    public float chaseRange = 6f;
    public int maxHp = 10;
    public int hp = 10;

    [Header("攻撃設定")]
    public float chargeTime = 1f;

    [Header("ノックバック設定")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    [Header("ランダム行動設定")]
    public bool enableWander = true;
    public float wanderInterval = 2f;
    public float wanderSpeedRate = 0.5f;

    [Header("被ダメージ設定")]
    public float damageInvulnerability = 0.25f;

    [Header("ドロップ設定")]
    public GameObject dropCrystalPrefab;
    public int dropAmount = 0;
    public int dropMin = 1;
    public int dropMax = 1;
    [Range(0f, 1f)]
    public float dropChance = 1.0f;

    protected Transform player;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected bool isAttacking = false;
    protected bool isWandering = false;
    protected bool isCharging = false;
    protected bool isKnockedBack = false;
    protected bool isFlashing = false;

    protected float attackTimer = 0f;
    protected Vector2 wanderDirection = Vector2.zero;
    protected float lastDamageTime = -999f;

    // =========================
    // ★追加：向き管理
    // =========================
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    protected FacingDirection facing = FacingDirection.Down; // ★追加

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError($"{name}: Player not found! Tag 'Player' must be set.");
        }

        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        if (enableWander)
        {
            StartCoroutine(WanderRoutine());
        }
    }

    protected virtual void Update()
    {
        if (player == null || isKnockedBack) return;

        attackTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (isAttacking || isCharging) return;

        if (distance <= chaseRange)
        {
            if (distance <= attackRange && attackTimer <= 0f)
            {
                StartCoroutine(AttackRoutine());
            }
            else if (attackTimer > 0f)
            {
                MaintainDistance();
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
        else if (isWandering)
        {
            transform.position += (Vector3)wanderDirection * speed * wanderSpeedRate * Time.deltaTime;
            if (wanderDirection == Vector2.zero)
                PlayIdleAnimation();
            else
            {
                UpdateFacing(wanderDirection);      // ★追加
                UpdateAnimation(wanderDirection);
            }
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        UpdateFacing(direction);                    // ★追加
        UpdateAnimation(direction);
    }

    protected virtual void MaintainDistance()
    {
        if (player == null) return;

        Vector2 direction = (transform.position - player.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < stopDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                (Vector2)player.position + direction * stopDistance,
                speed * Time.deltaTime
            );
        }
        else if (distance > stopDistance + 0.5f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }

        UpdateFacing(-direction);                   // ★追加
        UpdateAnimation(-direction);
    }

    // =========================
    // ★追加：向きを更新
    // =========================
    protected void UpdateFacing(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            facing = dir.x > 0 ? FacingDirection.Right : FacingDirection.Left;
        else
            facing = dir.y > 0 ? FacingDirection.Up : FacingDirection.Down;
    }

    // =========================
    // ★追加：向きをVector2で取得
    // =========================
    protected Vector2 GetFacingVector()
    {
        switch (facing)
        {
            case FacingDirection.Up: return Vector2.up;
            case FacingDirection.Down: return Vector2.down;
            case FacingDirection.Left: return Vector2.left;
            case FacingDirection.Right: return Vector2.right;
        }
        return Vector2.down;
    }

    protected virtual IEnumerator AttackRoutine()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        PlayAttackAnimation(GetFacingVector());     // ★修正（向きを渡す）

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    protected virtual IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (!isAttacking && !isCharging)
            {
                int action = UnityEngine.Random.Range(0, 5);
                if (action == 0)
                    wanderDirection = Vector2.zero;
                else
                {
                    switch (UnityEngine.Random.Range(0, 4))
                    {
                        case 0: wanderDirection = Vector2.up; break;
                        case 1: wanderDirection = Vector2.down; break;
                        case 2: wanderDirection = Vector2.left; break;
                        case 3: wanderDirection = Vector2.right; break;
                    }
                }
                isWandering = true;
            }
            yield return new WaitForSeconds(wanderInterval);
        }
    }

    public virtual void TakeDamage(int damage, Vector2 hitSourcePosition)
    {
        TakeDamage(damage, hitSourcePosition, false);
    }

    public virtual void TakeDamage(int damage, Vector2 hitSourcePosition, bool isCritical = false)
    {
        if (Time.time - lastDamageTime < damageInvulnerability) return;
        lastDamageTime = Time.time;

        hp -= damage;

        AudioManager.Instance?.PlaySE(SEType.EnemyDamage);

        DamagePopupSpawner popupSpawner = GetComponent<DamagePopupSpawner>();
        if (popupSpawner != null)
            popupSpawner.CreatePopup(damage, isCritical);

        onHpChanged?.Invoke(hp, maxHp);

        Vector2 knockDir = (transform.position - (Vector3)hitSourcePosition).normalized;
        StartCoroutine(KnockbackRoutine(knockDir));

        if (sr != null)
        {
            StopCoroutine(nameof(DamageFlash));
            StartCoroutine(DamageFlash());
        }

        if (hp <= 0)
            Die();
    }

    protected virtual IEnumerator DamageFlash()
    {
        if (isFlashing || sr == null) yield break;
        isFlashing = true;

        Color originalColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;

        isFlashing = false;
    }

    protected virtual IEnumerator KnockbackRoutine(Vector2 direction)
    {
        if (rb == null) yield break;

        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    protected virtual void Die()
    {
        DropItem();
        onEnemyDead?.Invoke(this);
        Destroy(gameObject);
    }

    protected void DropItem()
    {
        if (dropCrystalPrefab == null) return;

        float dropMultiplier = 1f;
        if (PlayerStatus.Instance != null)
            dropMultiplier = PlayerStatus.Instance.GetDropRateMultiplier();

        if (UnityEngine.Random.value > dropChance * dropMultiplier) return;

        int count = dropAmount > 0
            ? dropAmount
            : UnityEngine.Random.Range(dropMin, Mathf.Max(dropMin, dropMax) + 1);

        count = Mathf.RoundToInt(count * dropMultiplier);

        for (int i = 0; i < count; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.3f;
            Instantiate(dropCrystalPrefab, (Vector2)transform.position + offset, Quaternion.identity);
        }
    }

    protected virtual void UpdateAnimation(Vector2 dir) { }
    protected virtual void PlayAttackAnimation(Vector2 dir) { }

    protected virtual void PlayIdleAnimation()
    {
        if (animator != null)
            animator.Play("Idle", -1, 0f);
    }
}

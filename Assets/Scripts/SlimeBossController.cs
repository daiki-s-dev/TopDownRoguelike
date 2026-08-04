using UnityEngine;
using System.Collections;

public class SlimeBossController : EnemyBase
{
    [Header("ボス基本状態")]
    public bool isDead = false;

    [Header("攻撃警告")]
    public GameObject attackWarningPrefab;
    private GameObject warningInstance;

    [Header("ジャンプ設定")]
    public float jumpForce = 8f;
    public float jumpInterval = 0.3f;

    [Header("弾幕設定")]
    public GameObject bulletPrefab;
    public int bulletCount = 8;
    public float bulletSpeed = 5f;

    [Header("死亡演出")]
    public float fadeDuration = 1.0f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (isDead) return;
        base.Update();
    }

    // ======================================
    // 攻撃ルーチン（3種類）
    // ======================================
    protected override IEnumerator AttackRoutine()
    {
        if (isDead) yield break;

        isAttacking = true;
        isCharging = true;
        attackTimer = attackCooldown;

        int attackType = Random.Range(0, 3);

        switch (attackType)
        {
            case 0:
                yield return SingleJumpAttack();
                break;
            case 1:
                yield return TripleJumpAttack();
                break;
            case 2:
                yield return JumpAndShootAttack();
                break;
        }

        isAttacking = false;
        isCharging = false;
    }

    // --------------------
    // ① 単発ジャンプ
    // --------------------
    private IEnumerator SingleJumpAttack()
    {
        ShowAttackWarning();
        yield return new WaitForSeconds(chargeTime);

        JumpToPlayer();
        PlayAttackAnim();

        yield return new WaitForSeconds(0.8f);
        HideAttackWarning();
    }

    // --------------------
    // ② 3連続ジャンプ
    // --------------------
    private IEnumerator TripleJumpAttack()
    {
        ShowAttackWarning();
        yield return new WaitForSeconds(chargeTime);

        for (int i = 0; i < 3; i++)
        {
            JumpToPlayer();
            PlayAttackAnim();
            yield return new WaitForSeconds(jumpInterval);
        }

        HideAttackWarning();
        yield return new WaitForSeconds(0.6f);
    }

    // --------------------
    // ③ ジャンプ＋弾幕
    // --------------------
    private IEnumerator JumpAndShootAttack()
    {
        ShowAttackWarning();
        yield return new WaitForSeconds(chargeTime);

        JumpToPlayer();
        PlayAttackAnim();
        ShootBullets();

        yield return new WaitForSeconds(0.8f);
        HideAttackWarning();
    }

    // ======================================
    // 共通処理
    // ======================================
    private void JumpToPlayer()
    {
        if (rb == null || player == null) return;

        rb.linearVelocity = Vector2.zero;
        Vector2 dir = (player.position - transform.position).normalized;
        rb.AddForce(dir * jumpForce, ForceMode2D.Impulse);
    }

    private void ShootBullets()
    {
        if (bulletPrefab == null) return;

        float step = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = step * i * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            GameObject bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.identity
            );

            Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
            if (brb != null)
                brb.linearVelocity = dir * bulletSpeed;
        }
    }

    private void ShowAttackWarning()
    {
        if (attackWarningPrefab == null) return;

        warningInstance = Instantiate(
            attackWarningPrefab,
            transform.position,
            Quaternion.identity
        );
        warningInstance.transform.localScale = Vector3.one * attackRange * 2f;
    }

    private void HideAttackWarning()
    {
        if (warningInstance == null) return;
        Destroy(warningInstance);
        warningInstance = null;
    }

    private void PlayAttackAnim()
    {
        if (animator == null || player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        PlayAttackAnimation(dir);
    }

    // ======================================
    // ダメージ・死亡
    // ======================================
    public override void TakeDamage(int damage, Vector2 hitSourcePosition)
    {
        if (isDead) return;

        hp -= damage;
        onHpChanged?.Invoke(hp, maxHp);

        Vector2 knockDir = (transform.position - (Vector3)hitSourcePosition).normalized;
        StartCoroutine(KnockbackRoutine(knockDir));

        if (sr != null)
            StartCoroutine(DamageFlash());

        if (hp <= 0)
            StartCoroutine(DieRoutine());
    }

    protected override void Die()
    {
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        if (isDead) yield break;
        isDead = true;

        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (var c in cols)
            c.enabled = false;

        HideAttackWarning();
        DropItem();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.Play("Dead");

        yield return new WaitForSeconds(0.8f);

        if (sr != null)
        {
            float t = 0f;
            Color c = sr.color;
            while (t < fadeDuration)
            {
                sr.color = new Color(c.r, c.g, c.b, Mathf.Lerp(1f, 0f, t / fadeDuration));
                t += Time.deltaTime;
                yield return null;
            }
        }

        onEnemyDead?.Invoke(this);
        Destroy(gameObject);
    }

    // ======================================
    // アニメーション
    // ======================================
    protected override void UpdateAnimation(Vector2 dir)
    {
        if (animator == null) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            animator.Play(dir.x > 0 ? "Walk_right" : "Walk_left");
        else
            animator.Play(dir.y > 0 ? "Walk_back" : "Walk_front");
    }

    protected override void PlayAttackAnimation(Vector2 dir)
    {
        if (animator == null) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            animator.Play(dir.x > 0 ? "Attack_right" : "Attack_left");
        else
            animator.Play(dir.y > 0 ? "Attack_back" : "Attack_front");
    }
}

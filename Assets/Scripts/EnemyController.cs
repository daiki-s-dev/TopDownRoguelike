using System.Collections;
using UnityEngine;

/// <summary>
/// 通常の敵のコントローラー。
/// 現在の向きに合わせて攻撃警告を表示し、チャージ後に攻撃する。
/// </summary>
public class EnemyController : EnemyBase
{
    [Header("敵専用設定")]
    public GameObject attackWarningPrefab;
    private GameObject warningInstance;

    [Header("死亡演出設定")]
    public float fadeDuration = 1.0f;

    private bool isDead = false;

    [Header("EnemyAttack参照")]
    public EnemyAttack enemyAttack;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (isDead) return;
        base.Update();
    }

    #region 攻撃

    protected override IEnumerator AttackRoutine()
    {
        if (isDead) yield break;

        isAttacking = true;
        isCharging = true;
        attackTimer = attackCooldown;

        // 現在の向き（EnemyBase管理）
        Vector2 dir = GetFacingVector();

        // 攻撃警告（位置は変えず、向きだけ）
        if (attackWarningPrefab != null)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            warningInstance = Instantiate(
                attackWarningPrefab,
                transform.position,
                Quaternion.Euler(0f, 0f, angle),
                transform            // 親を敵にする
            );

            if (enemyAttack != null)
                enemyAttack.attackWarning = warningInstance;

            warningInstance.SetActive(true);
        }

        // チャージ中も回転だけ追従
        float timer = 0f;
        while (timer < chargeTime)
        {
            if (warningInstance != null)
            {
                Vector2 d = GetFacingVector();
                float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
                warningInstance.transform.rotation =
                    Quaternion.Euler(0f, 0f, angle);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 攻撃アニメ
        PlayAttackAnimation(GetFacingVector());

        yield return new WaitForSeconds(0.8f);

        // 後処理
        if (warningInstance != null)
        {
            Destroy(warningInstance);
            warningInstance = null;

            if (enemyAttack != null)
                enemyAttack.attackWarning = null;
        }

        isAttacking = false;
        isCharging = false;
    }

    #endregion

    #region アニメーション

    protected override void UpdateAnimation(Vector2 dir)
    {
        if (isDead || animator == null) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            animator.Play(dir.x > 0 ? "Walk_right" : "Walk_left");
        else
            animator.Play(dir.y > 0 ? "Walk_back" : "Walk_front");
    }

    protected override void PlayAttackAnimation(Vector2 dir)
    {
        if (isDead || animator == null) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            animator.Play(dir.x > 0 ? "Attack_right" : "Attack_left");
        else
            animator.Play(dir.y > 0 ? "Attack_back" : "Attack_front");
    }

    #endregion

    #region ダメージ・死亡

    public override void TakeDamage(int damage, Vector2 hitSourcePosition)
    {
        if (isDead) return;

        hp -= damage;
        onHpChanged?.Invoke(hp, maxHp);

        Vector2 knockDir =
            (transform.position - (Vector3)hitSourcePosition).normalized;
        StartCoroutine(KnockbackRoutine(knockDir));

        if (sr != null)
        {
            StopCoroutine(nameof(DamageFlash));
            StartCoroutine(DamageFlash());
        }

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

        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        if (warningInstance != null)
        {
            Destroy(warningInstance);
            warningInstance = null;

            if (enemyAttack != null)
                enemyAttack.attackWarning = null;
        }

        DropItem();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        animator.Play("Dead", 0, 0f);
        yield return new WaitForSeconds(GetDeathClipLength());

        if (sr != null)
        {
            float t = 0f;
            Color c = sr.color;
            while (t < fadeDuration)
            {
                sr.color = new Color(c.r, c.g, c.b, 1f - t / fadeDuration);
                t += Time.deltaTime;
                yield return null;
            }
        }

        onEnemyDead?.Invoke(this);
        Destroy(gameObject);
    }

    private float GetDeathClipLength()
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return 0.8f;

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
            if (clip.name == "Dead")
                return clip.length;

        return 0.8f;
    }

    #endregion
}
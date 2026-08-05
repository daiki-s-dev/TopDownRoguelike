using System.Collections;
using UnityEngine;

/// <summary>
/// 遠距離攻撃を行う通常の敵コントローラー。
/// 方向指定の弾攻撃、または範囲攻撃のいずれかを行う。
/// </summary>
public class RangedEnemyController : EnemyBase
{
    public enum AttackType
    {
        DirectionProjectile,
        AreaAttack
    }

    [Header("攻撃タイプ")]
    public AttackType attackType = AttackType.DirectionProjectile;

    [Header("プレハブ")]
    public GameObject projectilePrefab;
    public GameObject directionWarningPrefab;
    public GameObject areaWarningPrefab;
    public GameObject areaAttackPrefab;

    [Header("攻撃設定")]
    public float projectileSpeed = 6f;
    public float warningDuration = 1.0f;
    public float areaRadius = 1.5f;

    [Header("死亡演出設定")]
    public float fadeDuration = 1.0f;

    private GameObject warningInstance;
    private Vector2 lockedDirection;
    private Vector2 lockedPosition;

    private bool isDead = false;

    protected override void Update()
    {
        if (isDead) return;
        base.Update();
    }

    #region 攻撃

    protected override IEnumerator AttackRoutine()
    {
        if (isDead || isAttacking || player == null) yield break;

        isAttacking = true;
        isCharging = true;
        attackTimer = attackCooldown;

        // 攻撃情報ロック
        if (attackType == AttackType.DirectionProjectile)
        {
            lockedDirection = (player.position - transform.position).normalized;
            UpdateFacing(lockedDirection);
            CreateDirectionWarning(lockedDirection);
        }
        else
        {
            lockedPosition = player.position;
            CreateAreaWarning(lockedPosition);
        }

        // チャージ
        yield return new WaitForSeconds(warningDuration);

        // 警告削除
        ClearWarning();

        // 攻撃実行
        PlayAttackAnimation(GetFacingVector());

        if (attackType == AttackType.DirectionProjectile)
            FireProjectile();
        else
            SpawnAreaAttack();

        yield return new WaitForSeconds(0.3f);

        isAttacking = false;
        isCharging = false;
    }

    #endregion

    #region ①方向攻撃

    private void CreateDirectionWarning(Vector2 dir)
    {
        if (directionWarningPrefab == null) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        warningInstance = Instantiate(
            directionWarningPrefab,
            transform.position,
            Quaternion.Euler(0f, 0f, angle)
        );
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = lockedDirection * projectileSpeed;
    }

    #endregion

    #region ②範囲攻撃

    private void CreateAreaWarning(Vector2 pos)
    {
        if (areaWarningPrefab == null) return;

        warningInstance = Instantiate(
            areaWarningPrefab,
            pos,
            Quaternion.identity
        );

        warningInstance.transform.localScale =
            Vector3.one * areaRadius * 2f;
    }

    private void SpawnAreaAttack()
    {
        if (areaAttackPrefab == null) return;

        Instantiate(
            areaAttackPrefab,
            lockedPosition,
            Quaternion.identity
        );
    }

    private void ClearWarning()
    {
        if (warningInstance != null)
        {
            Destroy(warningInstance);
            warningInstance = null;
        }
    }

    #endregion

    #region 死亡処理（SlimeController互換）

    public override void TakeDamage(int damage, Vector2 hitSourcePosition)
    {
        if (isDead) return;

        base.TakeDamage(damage, hitSourcePosition);

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

        ClearWarning();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // 魔石ドロップ
        DropItem();

        if (animator != null)
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
}
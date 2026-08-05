using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離攻撃型ボスのコントローラー。
/// 複数の攻撃パターン（弾幕/範囲攻撃）をランダムに選択して実行する。
/// </summary>
public class RangedBossController : EnemyBase
{
    #region 攻撃タイプ

    public enum AttackType
    {
        MultiProjectile,
        AreaAttack
    }

    #endregion

    #region 攻撃パターン定義

    [System.Serializable]
    public class AttackPattern
    {
        public AttackType type;

        [Header("プレハブ")]
        public GameObject projectilePrefab;
        public GameObject directionWarningPrefab;
        public GameObject areaWarningPrefab;
        public GameObject areaAttackPrefab;

        [Header("弾攻撃設定")]
        public int projectileCount = 1;
        public float projectileSpeed = 6f;
        public float spreadAngle = 30f;

        [Header("範囲攻撃サイズ")]
        public float warningRadius = 2.5f; // 見た目用
        public float attackRadius = 2.0f;  // 当たり判定用

        [Header("警告")]
        public float warningDuration = 1.0f;
    }

    #endregion

    [Header("攻撃パターンリスト")]
    public List<AttackPattern> attackPatterns = new List<AttackPattern>();

    [Header("死亡演出")]
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

    #region 攻撃ルーチン

    protected override IEnumerator AttackRoutine()
    {
        if (isDead || attackPatterns.Count == 0 || player == null)
            yield break;

        isAttacking = true;
        isCharging = true;
        attackTimer = attackCooldown;

        AttackPattern pattern =
            attackPatterns[Random.Range(0, attackPatterns.Count)];

        if (pattern.type == AttackType.MultiProjectile)
        {
            lockedDirection = (player.position - transform.position).normalized;
            UpdateFacing(lockedDirection);
            CreateDirectionWarning(pattern);
        }
        else
        {
            lockedPosition = player.position;
            CreateAreaWarning(pattern);
        }

        yield return new WaitForSeconds(pattern.warningDuration);
        ClearWarning();

        PlayAttackAnimation(GetFacingVector());

        if (pattern.type == AttackType.MultiProjectile)
            FireProjectiles(pattern);
        else
            SpawnAreaAttack(pattern);

        yield return new WaitForSeconds(0.3f);

        isAttacking = false;
        isCharging = false;
    }

    #endregion

    #region 弾攻撃

    private void CreateDirectionWarning(AttackPattern p)
    {
        if (p.directionWarningPrefab == null) return;

        float angle = Mathf.Atan2(lockedDirection.y, lockedDirection.x) * Mathf.Rad2Deg;
        warningInstance = Instantiate(
            p.directionWarningPrefab,
            transform.position,
            Quaternion.Euler(0, 0, angle)
        );
    }

    private void FireProjectiles(AttackPattern p)
    {
        if (p.projectilePrefab == null) return;

        float startAngle = -p.spreadAngle * 0.5f;
        float step = p.projectileCount > 1
            ? p.spreadAngle / (p.projectileCount - 1)
            : 0f;

        for (int i = 0; i < p.projectileCount; i++)
        {
            float angle = startAngle + step * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * lockedDirection;

            GameObject proj = Instantiate(
                p.projectilePrefab,
                transform.position,
                Quaternion.identity
            );

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir.normalized * p.projectileSpeed;
        }
    }

    #endregion

    #region 範囲攻撃

    private void CreateAreaWarning(AttackPattern p)
    {
        if (p.areaWarningPrefab == null) return;

        warningInstance = Instantiate(
            p.areaWarningPrefab,
            lockedPosition,
            Quaternion.identity
        );

        // 警告用サイズ
        warningInstance.transform.localScale =
            Vector3.one * p.warningRadius * 2f;
    }

    private void SpawnAreaAttack(AttackPattern p)
    {
        if (p.areaAttackPrefab == null) return;

        GameObject atk = Instantiate(
            p.areaAttackPrefab,
            lockedPosition,
            Quaternion.identity
        );

        // 攻撃判定サイズ
        atk.transform.localScale =
            Vector3.one * p.attackRadius * 2f;
    }

    private void ClearWarning()
    {
        if (warningInstance != null)
            Destroy(warningInstance);
    }

    #endregion

    #region 死亡処理

    protected override void Die()
    {
        if (isDead) return;
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        if (isDead) yield break;
        isDead = true;

        // コライダー無効化
        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        // 攻撃警告削除
        ClearWarning();

        // 魔石ドロップ
        DropItem();

        // 移動停止
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // 死亡アニメ
        if (animator != null)
            animator.Play("Dead");

        yield return new WaitForSeconds(0.8f);

        // フェードアウト
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

    #endregion
}
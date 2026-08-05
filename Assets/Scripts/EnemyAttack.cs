using UnityEngine;

/// <summary>
/// 敵の攻撃判定・警告円のON/OFFをアニメーションイベントから制御する。
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    [Header("攻撃範囲オブジェクト")]
    public GameObject attackArea; // 攻撃判定オブジェクト

    [Header("攻撃警告オブジェクト")]
    public GameObject attackWarning; // EnemyController から渡される警告円

    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (attackArea != null)
            attackArea.SetActive(false);

        if (attackWarning != null)
            attackWarning.SetActive(false);
    }

    /// <summary>
    /// 攻撃開始（EnemyController から呼ばれる）。
    /// </summary>
    public void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// アニメーションイベント: 攻撃判定ON。
    /// </summary>
    public void StartAttackHitbox()
    {
        if (attackArea != null)
            attackArea.SetActive(true);
    }

    /// <summary>
    /// アニメーションイベント: 攻撃判定OFF（攻撃終了）。
    /// </summary>
    public void EndAttackHitbox()
    {
        if (attackArea != null)
            attackArea.SetActive(false);

        if (attackWarning != null)
        {
            attackWarning.SetActive(false); // 攻撃判定と同時に警告円を消す
            attackWarning = null;           // 参照をクリア
        }
    }

    /// <summary>
    /// アニメーションイベント: 攻撃終了。
    /// </summary>
    public void EndAttack()
    {
        isAttacking = false;
    }
}
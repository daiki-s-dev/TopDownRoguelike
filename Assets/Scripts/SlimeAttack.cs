using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    [Header("攻撃範囲オブジェクト")]
    public GameObject attackArea; // 攻撃判定オブジェクト

    [Header("攻撃警告オブジェクト")]
    public GameObject attackWarning; // SlimeController から渡される警告円

    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (attackArea != null)
            attackArea.SetActive(false);

        if (attackWarning != null)
            attackWarning.SetActive(false);
    }

    // 攻撃開始（SlimeController から呼ばれる）
    public void StartAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    // アニメーションイベント: 攻撃判定ON
    public void StartAttackHitbox()
    {
        if (attackArea != null)
            attackArea.SetActive(true);
    }

    // アニメーションイベント: 攻撃判定OFF（攻撃終了）
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

    // アニメーションイベント: 攻撃終了
    public void EndAttack()
    {
        isAttacking = false;
    }
}

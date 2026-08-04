using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    [Header("攻撃判定オブジェクト")]
    public GameObject attackArea; // AttackArea をここにアタッチ

    private Animator animator;
    private bool isAttacking = false;

    [Header("武器データ")]
    public WeaponData weaponData;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (attackArea != null)
            attackArea.SetActive(false); // 初期状態は非表示
    }

    void Update()
    {
        // クリックされたら攻撃
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    // アニメーションイベントで呼ばれる：攻撃判定ON
    public void StartAttackHitbox()
    {
        if (weaponData != null && weaponData.attackSE != null)
        {
            AudioSource.PlayClipAtPoint(weaponData.attackSE, transform.position);
        }

        if (attackArea != null)
            attackArea.SetActive(true);
    }

    // アニメーションイベントで呼ばれる：攻撃判定OFF
    public void EndAttackHitbox()
    {
        if (attackArea != null)
            attackArea.SetActive(false);
    }

    // アニメーション終了時に呼ぶ：攻撃終了
    public void EndAttack()
    {
        isAttacking = false;
    }


}

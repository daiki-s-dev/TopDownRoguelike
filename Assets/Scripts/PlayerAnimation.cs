using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーのアニメーション制御。
/// 移動・向き・被ダメージ・死亡演出を管理する。
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    public Transform playerBody;

    private Animator animator;
    private SpriteRenderer sr;
    private bool isDead = false;
    private bool isRightFacing = true; // 最後に向いていた方向を記録

    #region Unity Lifecycle

    private void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isDead) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        bool isWalking = (moveX != 0f || moveY != 0f);
        animator.SetBool("isWalking", isWalking);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        bool isRight = mousePos.x >= playerBody.position.x;
        isRightFacing = isRight;
        animator.SetBool("isRight", isRight);
    }

    #endregion

    #region 被ダメージ

    /// <summary>
    /// 被ダメージ時の再生処理。
    /// </summary>
    public void TakeDamage(Vector2 hitDirection)
    {
        if (isDead) return;
        StartCoroutine(HitFlashAndAnimation(hitDirection));
    }

    private IEnumerator HitFlashAndAnimation(Vector2 dir)
    {
        sr.color = Color.red;

        // 被ダメージアニメを即時再生
        string animName;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            animName = dir.x > 0 ? "PlayerDamage_left" : "PlayerDamage_right";
        else
            animName = dir.y > 0 ? "PlayerDamage_left" : "PlayerDamage_right";

        animator.Play(animName, 0, 0f);

        // 再生時間分だけ待機
        yield return new WaitForSeconds(GetClipLength(animName));

        sr.color = Color.white;
    }

    #endregion

    #region 死亡・復帰

    /// <summary>
    /// 死亡処理。
    /// </summary>
    public void PlayDeathAnimation(Vector2 hitDirection)
    {
        if (isDead) return;
        isDead = true;

        // 向きを固定
        animator.SetBool("isRight", isRightFacing);
        animator.Update(0f);
        animator.SetBool("isDead", true);

        // 死亡アニメ再生
        animator.Play(isRightFacing ? "PlayerDead_right" : "PlayerDead_left", 0, 0f);

        Debug.Log($"死亡アニメ固定再生: {(isRightFacing ? "右向き" : "左向き")}");
    }

    public void ResetAnimation()
    {
        isDead = false;
        animator.SetBool("isDead", false);
        animator.SetBool("isWalking", false);

        // 向きは最後の向きに固定
        animator.SetBool("isRight", isRightFacing);

        // 必要なら Idle アニメ再生
        animator.Play(isRightFacing ? "PlayerIdle_right" : "PlayerIdle_left", 0, 0f);
    }

    #endregion

    #region ユーティリティ

    /// <summary>
    /// アニメーションクリップの長さ取得。
    /// </summary>
    private float GetClipLength(string clipName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return 0.5f;

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 0.5f;
    }

    #endregion
}
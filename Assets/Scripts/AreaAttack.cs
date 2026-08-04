using System.Collections;
using UnityEngine;

/// <summary>
/// 敵などが発生させる範囲攻撃。
/// 出現後、少し遅れて当たり判定が有効になり、一定時間で消滅する。
/// </summary>
public class AreaAttack : MonoBehaviour
{
    [Header("範囲攻撃設定")]
    public int damage = 5;
    public float lifeTime = 0.5f;  // 表示されている時間
    public float hitDelay = 0.1f;  // 発生から当たり判定までの遅延

    private Collider2D col;
    private bool hasHit = false;   // 1回ヒット管理フラグ

    #region Unity Lifecycle

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        // 警告 → 本体演出用に、最初は当たり判定オフ
        if (col != null)
            col.enabled = false;
    }

    private void Start()
    {
        // 少し遅らせて当たり判定ON
        StartCoroutine(EnableHit());

        // 一定時間後に自動消滅
        Destroy(gameObject, lifeTime);
    }

    #endregion

    #region 当たり判定

    private IEnumerator EnableHit()
    {
        yield return new WaitForSeconds(hitDelay);

        if (col != null)
            col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (!other.CompareTag("Player")) return;

        PlayerStatus player = other.GetComponent<PlayerStatus>();
        if (player == null) return;

        hasHit = true;

        // 範囲攻撃の中心をヒット元として渡す
        Vector2 hitSource = transform.position;
        player.TakeDamage(damage, hitSource);
    }

    #endregion
}
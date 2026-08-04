using UnityEngine;
using System.Collections;

public class AreaAttack : MonoBehaviour
{
    [Header("範囲攻撃設定")]
    public int damage = 5;
    public float lifeTime = 0.5f;   // 表示されている時間
    public float hitDelay = 0.1f;    // 発生から当たり判定までの遅延

    private Collider2D col;
    private bool hasHit = false;     // ★ 1回ヒット管理フラグ

    void Awake()
    {
        col = GetComponent<Collider2D>();

        // 警告 → 本体演出用に最初は当たり判定オフ
        if (col != null)
            col.enabled = false;
    }

    void Start()
    {
        // 少し遅らせて当たり判定ON
        StartCoroutine(EnableHit());

        // 一定時間後に自動消滅
        Destroy(gameObject, lifeTime);
    }

    IEnumerator EnableHit()
    {
        yield return new WaitForSeconds(hitDelay);

        if (col != null)
            col.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (!other.CompareTag("Player")) return;

        PlayerStatus player = other.GetComponent<PlayerStatus>();
        if (player != null)
        {
            hasHit = true;

            // 範囲攻撃の中心をヒット元として渡す
            Vector2 hitSource = transform.position;
            player.TakeDamage(damage, hitSource);
        }
    }
}

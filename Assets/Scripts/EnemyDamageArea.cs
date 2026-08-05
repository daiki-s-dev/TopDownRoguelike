using UnityEngine;

/// <summary>
/// 敵の攻撃判定エリア。
/// 有効化された瞬間に範囲内のプレイヤーを判定し、1回の攻撃につき1回だけダメージを与える。
/// </summary>
public class EnemyDamageArea : MonoBehaviour
{
    [Header("攻撃ダメージ")]
    public int damage = 5;

    [Header("攻撃対象のタグ")]
    public string targetTag = "Player";

    private SpriteRenderer sr;
    private bool hasDealtDamage = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 攻撃範囲ON/OFFを制御する（外部から呼ばれる）。
    /// </summary>
    public void EnableDamage(bool enable)
    {
        if (sr != null)
            sr.enabled = enable;

        gameObject.SetActive(enable);

        if (enable)
            hasDealtDamage = false; // 新しい攻撃開始
    }

    /// <summary>
    /// 攻撃エリアが有効化された瞬間に中のプレイヤーを強制チェックする。
    /// </summary>
    private void OnEnable()
    {
        hasDealtDamage = false;

        Collider2D[] results = new Collider2D[10];
        int count = Physics2D.OverlapCollider(
            GetComponent<Collider2D>(),
            new ContactFilter2D().NoFilter(),
            results
        );

        for (int i = 0; i < count; i++)
        {
            Collider2D hit = results[i];
            if (hit != null && hit.CompareTag(targetTag))
            {
                DealDamage(hit);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DealDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        DealDamage(other);
    }

    private void DealDamage(Collider2D other)
    {
        if (hasDealtDamage) return;
        if (!gameObject.activeSelf) return;

        if (other.CompareTag(targetTag))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                player.TakeDamage(damage, transform.position);
                Debug.Log($"敵の攻撃：プレイヤーに {damage} ダメージ！");
                hasDealtDamage = true;
            }
        }
    }
}
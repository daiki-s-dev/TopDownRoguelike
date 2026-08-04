using UnityEngine;

public class SlimeDamageArea : MonoBehaviour
{
    [Header("攻撃ダメージ")]
    public int damage = 5;

    [Header("攻撃対象のタグ")]
    public string targetTag = "Player";

    private SpriteRenderer sr;
    private bool hasDealtDamage = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // 攻撃範囲ON/OFFを制御（外部から呼ばれる）
    public void EnableDamage(bool enable)
    {
        if (sr != null)
            sr.enabled = enable;

        gameObject.SetActive(enable);

        if (enable)
            hasDealtDamage = false; // 新しい攻撃開始
    }

    // 攻撃エリアが有効化された瞬間に中のプレイヤーを強制チェック
    void OnEnable()
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

    void OnTriggerEnter2D(Collider2D other)
    {
        DealDamage(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        DealDamage(other);
    }

    void DealDamage(Collider2D other)
    {
        if (hasDealtDamage) return;
        if (!gameObject.activeSelf) return;

        if (other.CompareTag(targetTag))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                player.TakeDamage(damage, transform.position);
                Debug.Log($"スライムの攻撃：プレイヤーに {damage} ダメージ！");
                hasDealtDamage = true;
            }
        }
    }
}

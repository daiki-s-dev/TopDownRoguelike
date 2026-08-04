using UnityEngine;

/// <summary>
/// 直進する魔法の弾。敵に当たるとダメージを与えて消滅し、
/// 壁に当たった場合は刺さらずそのまま消滅する。
/// </summary>
public class MagicProjectile : MonoBehaviour
{
    [Header("移動・寿命設定")]
    public float speed = 10f;
    public float lifeTime = 3f;

    private int damage;
    private bool isCritical;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(int dmg, bool critical)
    {
        damage = dmg;
        isCritical = critical;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 敵に当たったらダメージ
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
                enemy.TakeDamage(damage, transform.position, isCritical);

            Destroy(gameObject);
            return;
        }

        // 壁に当たったら消える（刺さらない）
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

/// <summary>
/// 弓による矢の弾。敵に当たるとダメージを与えて消滅し、
/// 壁に当たると刺さって一定時間後に消滅する。
/// </summary>
public class Arrow : MonoBehaviour
{
    [Header("移動・寿命設定")]
    public float speed = 12f;
    public float lifeTime = 5f;
    public float stickTime = 1.5f;

    private int damage;
    private bool isCritical;
    private bool isStuck;

    private Rigidbody2D rb;
    private Collider2D col;

    #region Unity Lifecycle

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (isStuck) return;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    #endregion

    #region 初期化

    public void Init(int dmg, bool critical)
    {
        damage = dmg;
        isCritical = critical;
        Destroy(gameObject, lifeTime);
    }

    #endregion

    #region 当たり判定

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
                enemy.TakeDamage(damage, transform.position, isCritical); // 必ずここ

            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            StickToWall();
        }
    }

    private void StickToWall()
    {
        if (isStuck) return;
        isStuck = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        col.enabled = false;

        Destroy(gameObject, stickTime);
    }

    #endregion
}
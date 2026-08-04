using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 5f;
    public float stickTime = 1.5f;

    private int damage;
    private bool isCritical;
    private bool isStuck;

    Rigidbody2D rb;
    Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void Init(int dmg, bool critical)
    {
        damage = dmg;
        isCritical = critical;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (isStuck) return;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
                enemy.TakeDamage(damage, transform.position, isCritical); // ★必ずここ

            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            StickToWall();
        }
    }

    void StickToWall()
    {
        if (isStuck) return;
        isStuck = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        col.enabled = false;

        Destroy(gameObject, stickTime);
    }
}

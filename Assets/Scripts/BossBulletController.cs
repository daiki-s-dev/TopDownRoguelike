using UnityEngine;

/// <summary>
/// ボスが発射する弾。指定方向へ直進し、
/// プレイヤーか壁に当たると消滅する。
/// </summary>
public class BossBulletController : MonoBehaviour
{
    [Header("弾設定")]
    public float speed = 6f;
    public float lifeTime = 3f;
    public int damage = 1;

    private Vector2 moveDir;

    public void Init(Vector2 dir)
    {
        moveDir = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // PlayerHP ではなく PlayerStatus を使う
        if (other.CompareTag("Player"))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();
            if (player != null)
            {
                // 弾の位置をヒット元として渡す
                player.TakeDamage(damage, transform.position);
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
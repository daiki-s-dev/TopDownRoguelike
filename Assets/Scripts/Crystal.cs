using UnityEngine;

/// <summary>
/// 敵ドロップ等で出現するクリスタル。
/// プレイヤーが一定距離まで近づくと引き寄せられ、回収される。
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Crystal : MonoBehaviour
{
    [Header("引き寄せ設定")]
    public float magnetRadius = 3.0f;    // この距離以内で引き寄せが始まる
    public float magnetSpeed = 6.0f;     // 引き寄せ時の移動速度
    public float pickupDistance = 0.3f;  // この距離で回収扱い

    private Transform player;
    private Rigidbody2D rb;

    #region Unity Lifecycle

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Rigidbody2D は kinematic 推奨（物理で飛ばさない）
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;
        }

        // Collider は Trigger にしておくことを推奨
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            // もし設定が違ったら警告（自動で変えない）
            Debug.LogWarning($"{name}: Crystal の Collider2D は IsTrigger = true にしてください。");
        }
    }

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
            if (player == null) return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        // 一定距離で引き寄せ
        if (dist <= magnetRadius)
        {
            // 移動は Rigidbody2D を通すか transform.MoveTowards
            Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, magnetSpeed * Time.deltaTime);
            rb.MovePosition(newPos);

            // 近づきすぎたら回収
            if (dist <= pickupDistance)
            {
                Pickup();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    #endregion

    #region 回収処理

    private void Pickup()
    {
        // 回収処理：インベントリへ渡してログ出力
        if (PlayerCrystalInventory.Instance != null)
        {
            PlayerCrystalInventory.Instance.AddCrystal(1);
        }
        else
        {
            Debug.LogWarning("PlayerCrystalInventory がシーンに存在しません。AddCrystal を呼べませんでした。");
        }

        Destroy(gameObject);
    }

    #endregion
}
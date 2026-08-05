using UnityEngine;

/// <summary>
/// プレイヤーの移動入力と物理移動を制御する。
/// ノックバック中は入力・移動処理を無効化する。
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private PlayerStatus playerStatus;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        // ノックバック中は入力を無効化（動かさない）
        if (playerStatus != null && playerStatus.IsKnockedBack())
        {
            moveInput = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        // ノックバック中は物理制御をしない（上書きしない）
        if (playerStatus != null && playerStatus.IsKnockedBack())
        {
            return; // velocity をゼロにしない
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }
}
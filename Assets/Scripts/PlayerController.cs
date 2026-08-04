using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private PlayerStatus playerStatus;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStatus = GetComponent<PlayerStatus>();
    }

    void Update()
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

    void FixedUpdate()
    {
        // ノックバック中は物理制御をしない（上書きしない）
        if (playerStatus != null && playerStatus.IsKnockedBack())
        {
            return; // ← velocity をゼロにしない
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }
}

using UnityEngine;

/// <summary>
/// 宝箱とのインタラクション。
/// 開封アニメーション再生、中身排出、SE再生を行い、以降は再インタラクト不可にする。
/// </summary>
public class TreasureBoxInteract : MonoBehaviour, IInteractable
{
    [Header("宝箱ロジック")]
    public TreasureBox treasureBox;

    [Header("アニメーション")]
    public Animator animator;
    private static readonly int OpenHash = Animator.StringToHash("Open");

    [Header("説明UI（プレハブ内）")]
    public GameObject hintUI;

    [Header("SE再生")]
    public AudioManager audioManager; // ここでAudioManagerを参照

    private bool opened = false;

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // 初期状態は非表示
        if (hintUI != null)
            hintUI.SetActive(false);

        // AudioManagerがシーンにあれば自動取得
        if (audioManager == null)
            audioManager = AudioManager.Instance;
    }

    public string GetInteractName()
    {
        return "宝箱を開ける";
    }

    public void Interact(PlayerInventory inventory)
    {
        if (opened) return;

        opened = true;

        // Openアニメーション
        if (animator != null)
            animator.SetBool(OpenHash, true);

        // 中身排出
        treasureBox.Open();

        // 宝箱開封SE再生
        if (audioManager != null)
            audioManager.PlaySE(SEType.ChestOpen);

        // UI非表示 & 再インタラクト防止
        HideHint();
        DisableInteraction();
    }

    private void DisableInteraction()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;
        if (!other.CompareTag("Player")) return;

        ShowHint();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        HideHint();
    }

    private void ShowHint()
    {
        if (hintUI != null)
            hintUI.SetActive(true);
    }

    private void HideHint()
    {
        if (hintUI != null)
            hintUI.SetActive(false);
    }
}
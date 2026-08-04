using UnityEngine;

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

    void Reset()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
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

    void DisableInteraction()
    {
        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (opened) return;
        if (!other.CompareTag("Player")) return;

        ShowHint();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        HideHint();
    }

    void ShowHint()
    {
        if (hintUI != null)
            hintUI.SetActive(true);
    }

    void HideHint()
    {
        if (hintUI != null)
            hintUI.SetActive(false);
    }
}

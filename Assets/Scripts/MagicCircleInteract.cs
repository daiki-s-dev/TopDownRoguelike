using UnityEngine;

/// <summary>
/// プレイヤーが触れる魔法陣とのインタラクション。
/// 魔法陣UIの開閉を管理し、範囲外に出ると自動で閉じる。
/// </summary>
public class MagicCircleInteract : MonoBehaviour, IInteractable
{
    [Header("表示名")]
    [SerializeField] private string interactName = "魔法陣";

    [Header("魔法陣UI")]
    [SerializeField] private MagicCircleUIController magicCircleUI;

    // UIが開いているか
    private bool isOpen = false;

    private void Awake()
    {
        // UIが未設定ならシーン内から取得
        if (magicCircleUI == null)
        {
            magicCircleUI = FindFirstObjectByType<MagicCircleUIController>(FindObjectsInactive.Include);
        }

        if (magicCircleUI == null)
            Debug.LogError("MagicCircleUIController が見つかりません！");
    }

    #region IInteractable 実装

    public string GetInteractName()
    {
        return interactName;
    }

    public void Interact(PlayerInventory inventory)
    {
        if (isOpen) return;

        isOpen = true;
        magicCircleUI.Open(this);
    }

    #endregion

    /// <summary>
    /// UIから呼ばれる閉じる通知。
    /// </summary>
    public void OnUIClose()
    {
        isOpen = false;
    }

    /// <summary>
    /// 2D用：プレイヤーが範囲から離れたら自動でUIを閉じる。
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Trigger exited: {other.name}, isOpen={isOpen}");

        if (!isOpen) return;

        // プレイヤーかどうか判定
        if (other.TryGetComponent<PlayerInventory>(out var player))
        {
            Debug.Log("Closing MagicCircle UI");
            magicCircleUI.Close();
        }
    }
}
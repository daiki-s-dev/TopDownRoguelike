using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropMenuController : MonoBehaviour
{
    public static DropMenuController Instance;

    [Header("捨てるメニューのUI")]
    public GameObject menuUI;

    [Header("捨てるボタン")]
    public Button dropButton;

    private DragSlot currentSlot;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (menuUI != null)
            menuUI.SetActive(false);

        if (dropButton != null)
            dropButton.onClick.AddListener(OnDropButtonClicked);
    }

    // ★★★★★ メニューを開く（右クリックで呼ばれる）★★★★
    public void OpenMenu(DragSlot slot, Vector2 screenPos)
    {
        currentSlot = slot;

        menuUI.SetActive(true);

        // メニュー位置をマウスの位置に合わせる
        RectTransform rect = menuUI.GetComponent<RectTransform>();
        rect.position = screenPos;
    }

    // ★★★★★ メニューを閉じる ★★★★★
    public void CloseMenu()
    {
        menuUI.SetActive(false);
        currentSlot = null;
    }

    // ★★★★★ 捨てるボタンクリック処理 ★★★★★
    private void OnDropButtonClicked()
    {
        if (currentSlot == null) return;
        if (currentSlot.weaponData == null) return;

        // プレイヤーのインベントリ取得
        PlayerInventory inventory = Object.FindFirstObjectByType<PlayerInventory>();

        // アイテムをシーンにドロップ
        if (inventory != null)
        {
            inventory.DropWeapon(currentSlot.weaponData);
        }

        // スロットクリア
        currentSlot.ClearSlot();

        // メニューを閉じる
        CloseMenu();
    }

    void Update()
    {
        // メニューが開いていて、左クリックしたら閉じる（ボタン以外）
        if (menuUI.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(
                        menuUI.GetComponent<RectTransform>(),
                        Input.mousePosition))
                {
                    CloseMenu();
                }
            }
        }

        if (menuUI.activeSelf)
        {
            if (InventoryUIController.Instance != null &&
                !InventoryUIController.Instance.IsOpen)
            {
                CloseMenu();
            }
        }
    }
}

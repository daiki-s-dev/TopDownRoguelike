using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 装備スロットを右クリックした際に表示される「捨てる」メニューを管理するシングルトン。
/// </summary>
public class DropMenuController : MonoBehaviour
{
    public static DropMenuController Instance;

    [Header("捨てるメニューのUI")]
    public GameObject menuUI;

    [Header("捨てるボタン")]
    public Button dropButton;

    private DragSlot currentSlot;

    #region Unity Lifecycle

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (menuUI != null)
            menuUI.SetActive(false);

        if (dropButton != null)
            dropButton.onClick.AddListener(OnDropButtonClicked);
    }

    private void Update()
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

    #endregion

    #region メニュー開閉

    /// <summary>
    /// メニューを開く（右クリックで呼ばれる）。
    /// </summary>
    public void OpenMenu(DragSlot slot, Vector2 screenPos)
    {
        currentSlot = slot;

        menuUI.SetActive(true);

        // メニュー位置をマウスの位置に合わせる
        RectTransform rect = menuUI.GetComponent<RectTransform>();
        rect.position = screenPos;
    }

    public void CloseMenu()
    {
        menuUI.SetActive(false);
        currentSlot = null;
    }

    #endregion

    #region ボタン処理

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

    #endregion
}
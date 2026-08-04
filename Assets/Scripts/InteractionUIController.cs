using UnityEngine;
using System.Collections.Generic;

public class InteractionUIController : MonoBehaviour
{
    [Header("基本")]
    public RectTransform panel;         // ★ Destroy しない
    public Camera uiCamera;
    public Transform player;
    public Vector3 offset = new Vector3(80f, 0f, 0f);

    [Header("プレハブ")]
    public GameObject optionPrefab;

    private List<InteractOptionUI> options = new List<InteractOptionUI>();
    private int currentIndex = 0;

    void Awake()
    {
        if (panel != null)
            panel.gameObject.SetActive(false);
    }

    void Update()
    {
        // panel が無い（破棄済み）なら何もしない
        if (panel == null) return;

        // インベントリ or ポーズ中は非表示
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        bool pauseOpen = PauseMenuManager.IsPaused;

        if (inventoryOpen || pauseOpen)
        {
            Hide();
            return;
        }

        // プレイヤー追従
        if (player != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);
            panel.position = screenPos + offset;
        }

        HandleScrollInput();
        UpdateHighlight();
    }

    // ================================
    // 表示
    // ================================
    public void ShowOptions(List<string> names)
    {
        if (panel == null) return;

        ClearOptions();

        foreach (var n in names)
        {
            var go = Instantiate(optionPrefab, panel);
            var ui = go.GetComponent<InteractOptionUI>();
            ui.SetText(n);
            options.Add(ui);
        }

        currentIndex = 0;
        panel.gameObject.SetActive(options.Count > 0);
        UpdateHighlight();
    }

    public void Hide()
    {
        if (panel == null) return;

        ClearOptions();
        panel.gameObject.SetActive(false);
    }

    // ================================
    // 内部処理
    // ================================
    void ClearOptions()
    {
        foreach (var o in options)
        {
            if (o != null)
                Destroy(o.gameObject);   // ★ Destroy するのは option だけ
        }
        options.Clear();
    }

    void HandleScrollInput()
    {
        if (options.Count == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.UpArrow) || scroll > 0f)
            currentIndex--;

        if (Input.GetKeyDown(KeyCode.DownArrow) || scroll < 0f)
            currentIndex++;

        currentIndex = Mathf.Clamp(currentIndex, 0, options.Count - 1);
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < options.Count; i++)
        {
            bool selected = (i == currentIndex);
            if (options[i].background != null)
            {
                options[i].background.color = selected
                    ? new Color(0.3f, 0.55f, 1f, 0.9f)
                    : new Color(1f, 1f, 1f, 0.18f);
            }
        }
    }

    // ================================
    // PlayerInteract 用
    // ================================
    public string GetSelectedName()
    {
        if (options.Count == 0) return null;
        return options[currentIndex].label.text.Replace("[E] ", "");
    }
}

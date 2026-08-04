using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 画面に表示中の祝福アイコン一覧（バフUI）を管理するシングルトン。
/// インベントリやポーズメニューが開いている間は非表示にする。
/// </summary>
public class BlessingManager : MonoBehaviour
{
    public static BlessingManager Instance;

    [Header("UI")]
    public Transform panel;            // BlessingIcon の親
    public TextMeshProUGUI titleText;  // タイトル
    public GameObject blessingIconPrefab;

    private readonly Dictionary<BlessingType, BlessingUI> uiDict
        = new Dictionary<BlessingType, BlessingUI>();

    #region Unity Lifecycle

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // インベントリ or ポーズ中なら非表示
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        bool pauseOpen = PauseMenuManager.IsPaused;

        bool hideUI = inventoryOpen || pauseOpen;

        if (panel != null)
            panel.gameObject.SetActive(!hideUI);

        if (titleText != null)
            titleText.gameObject.SetActive(!hideUI);
    }

    #endregion

    #region UI更新

    public void UpdateBlessingUI(List<PlayerStatus.ActiveBlessing> activeBlessings)
    {
        if (panel == null || blessingIconPrefab == null) return;

        // 新規に取得した祝福のアイコンを追加
        foreach (var ab in activeBlessings)
        {
            if (uiDict.ContainsKey(ab.blessing.type)) continue;

            GameObject newIcon = Instantiate(blessingIconPrefab, panel);
            newIcon.transform.localScale = Vector3.one;

            BlessingUI ui = newIcon.GetComponent<BlessingUI>();
            if (ui != null)
            {
                ui.SetBlessing(ab.blessing);
                uiDict.Add(ab.blessing.type, ui);
            }
        }

        // 消えた祝福を削除
        List<BlessingType> toRemove = new List<BlessingType>();
        foreach (var kvp in uiDict)
        {
            if (!activeBlessings.Exists(x => x.blessing.type == kvp.Key))
                toRemove.Add(kvp.Key);
        }

        foreach (var type in toRemove)
        {
            Destroy(uiDict[type].gameObject);
            uiDict.Remove(type);
        }
    }

    public void ClearBlessingUI()
    {
        Debug.Log("ClearBlessingUI called");

        foreach (var ui in uiDict.Values)
        {
            if (ui != null)
                Destroy(ui.gameObject);
        }

        uiDict.Clear();
    }

    #endregion
}
// ============================================
// BlessingSelectUI.cs
// ============================================
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BlessingSelectUI : MonoBehaviour
{
    [Header("参照")]
    public PlayerStatus playerStatus;          // プレイヤー操作無効化用
    public Transform buttonParent;             // ボタンを生成する親
    public GameObject blessingButtonPrefab;    // ボタンPrefab
    public GameObject panel;                   // 背景パネル
    public TextMeshProUGUI descriptionText;    // 説明文表示用

    private List<Blessing> currentBlessings = new List<Blessing>();

    // ★ UI表示
    public void ShowBlessings(List<Blessing> blessings)
    {
        if (blessings == null || blessings.Count == 0) return;

        currentBlessings = blessings;

        ClearButtons(); // 既存ボタン削除

        foreach (var blessing in blessings)
        {
            GameObject btnObj = Instantiate(blessingButtonPrefab, buttonParent);

            BlessingButton btnScript = btnObj.GetComponent<BlessingButton>();
            if (btnScript != null)
            {
                btnScript.SetBlessing(blessing, this);
                btnScript.button.onClick.AddListener(() => OnBlessingSelected(blessing));
            }
        }

        // パネルと説明文初期化
        if (panel != null) panel.SetActive(true);
        if (descriptionText != null) descriptionText.gameObject.SetActive(false);

        // プレイヤー操作無効化
        if (playerStatus != null) playerStatus.enabled = false;
    }

    // ★ 祝福選択時
    private void OnBlessingSelected(Blessing selected)
    {
        if (playerStatus != null)
        {
            playerStatus.ApplyBlessing(selected);
            playerStatus.enabled = true;
        }

        if (panel != null) panel.SetActive(false);
        ClearButtons();

        if (GameManager.Instance != null)
            GameManager.Instance.LoadNextFloor();
    }

    // ★ 説明文表示
    public void ShowDescription(string desc)
    {
        if (descriptionText != null)
        {
            descriptionText.text = desc;
            descriptionText.gameObject.SetActive(true);
        }
    }

    public void HideDescription()
    {
        if (descriptionText != null)
            descriptionText.gameObject.SetActive(false);
    }

    // ★ ボタン削除
    private void ClearButtons()
    {
        if (buttonParent == null) return;

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }
    }

    // ★ 強制非表示
    public void HideUI()
    {
        if (panel != null) panel.SetActive(false);
        if (playerStatus != null) playerStatus.enabled = true;
        HideDescription();
        ClearButtons();
    }
}

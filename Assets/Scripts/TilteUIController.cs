using UnityEngine;

/// <summary>
/// タイトル画面のUIを管理する。
/// 通常メニューとHowTo（遊び方）パネルの切り替え、ゲーム開始を担当する。
/// </summary>
public class TitleUIController : MonoBehaviour
{
    [Header("Titleの通常UI (Start、HowToボタン等)")]
    [SerializeField] private GameObject titleButtons;

    [Header("HowToパネル")]
    [SerializeField] private GameObject howToPanel;

    private void Start()
    {
        // 初期状態は Titleボタン表示 / HowTo閉じる
        titleButtons.SetActive(true);
        howToPanel.SetActive(false);
    }

    /// <summary>
    /// HowTo ボタンを押したとき。
    /// </summary>
    public void OnClickHowTo()
    {
        titleButtons.SetActive(false);   // Title UI を消す
        howToPanel.SetActive(true);      // HowTo を開く
    }

    /// <summary>
    /// Close ボタンで Title に戻る。
    /// </summary>
    public void OnClickCloseHowTo()
    {
        howToPanel.SetActive(false);     // HowTo を閉じる
        titleButtons.SetActive(true);    // Title UI を再表示
    }

    /// <summary>
    /// ゲーム開始（Startボタン）。
    /// </summary>
    public void OnClickStart()
    {
        // SceneController が存在しないとエラーになるため安全チェック
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadLobby();
        }
        else
        {
            Debug.LogError("SceneController.Instance が見つかりません。Hierarchy に SceneController を配置してください。");
        }
    }
}
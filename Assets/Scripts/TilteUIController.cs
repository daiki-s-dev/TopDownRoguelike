using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    [Header("Titleの通常UI (Start、HowToボタン等)")]
    [SerializeField] private GameObject titleButtons;

    [Header("HowToパネル")]
    [SerializeField] private GameObject howToPanel;

    void Start()
    {
        // 初期状態は Titleボタン表示 / HowTo閉じる
        titleButtons.SetActive(true);
        howToPanel.SetActive(false);
    }

    // ▼ HowTo ボタンを押したとき
    public void OnClickHowTo()
    {
        titleButtons.SetActive(false);   // Title UI を消す
        howToPanel.SetActive(true);      // HowTo を開く
    }

    // ▼ Close ボタンで Title に戻る
    public void OnClickCloseHowTo()
    {
        howToPanel.SetActive(false);     // HowTo を閉じる
        titleButtons.SetActive(true);    // Title UI を再表示
    }

    // ▼ ゲーム開始（Startボタン）
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

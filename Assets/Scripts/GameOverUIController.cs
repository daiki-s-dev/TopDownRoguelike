using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバー画面のUIを管理する。
/// ロビー/タイトルへ戻る際にプレイヤー状態のリセットとセッションのリセットを行う。
/// </summary>
public class GameOverUIController : MonoBehaviour
{
    public GameObject gameOverPanel;

    private GameManager gameManager;
    private PlayerStatus playerStatus;
    private PlayerCrystalInventory crystalInventory;

    private void Start()
    {
        // 最初は非表示
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // シングルトン参照を取得
        gameManager = GameManager.Instance;
        playerStatus = PlayerStatus.Instance;
        crystalInventory = PlayerCrystalInventory.Instance;

        if (gameManager == null)
            Debug.LogError("GameOverUIController: GameManager が見つかりません！");
        if (playerStatus == null)
            Debug.LogError("GameOverUIController: PlayerStatus が見つかりません！");
        if (crystalInventory == null)
            Debug.LogWarning("PauseMenuManager: PlayerCrystalInventory が見つかりません！");
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Time.timeScale は変更せずゲームは動かしたまま
    }

    public void OnLobbyButton()
    {
        // プレイヤー復活・ステータス初期化
        if (playerStatus != null)
            playerStatus.ResetStatus();

        // 必要に応じて GameManager 側にも通知
        if (gameManager != null)
            gameManager.ExitDungeon();

        crystalInventory?.ResetCurrentSession();

        SceneManager.LoadScene("LobbyScene");
    }

    public void OnTitleButton()
    {
        if (playerStatus != null)
            playerStatus.ResetStatus();

        if (gameManager != null)
            gameManager.ExitDungeon();

        crystalInventory?.ResetCurrentSession();

        SceneManager.LoadScene("TitleScene");
    }
}
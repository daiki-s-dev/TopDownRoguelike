using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// クリアシーンでのボタン操作（ロビー/タイトルへ戻る）を管理する。
/// 戻る前にプレイヤーの状態リセットやクリスタルの預け入れを行う。
/// </summary>
public class ClearSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerStatus playerStatus;
    private PlayerCrystalInventory crystalInventory;

    private void Start()
    {
        // DontDestroyOnLoad の GameManager と Player を参照
        gameManager = GameManager.Instance;
        playerStatus = PlayerStatus.Instance;
        crystalInventory = PlayerCrystalInventory.Instance;

        if (gameManager == null)
            Debug.LogError("ClearSceneManager: GameManager が見つかりません！");
        if (playerStatus == null)
            Debug.LogError("ClearSceneManager: PlayerStatus が見つかりません！");
        if (crystalInventory == null)
            Debug.LogError("ClearSceneManager: PlayerCrystalInventory が見つかりません！");
    }

    /// <summary>
    /// Lobbyシーンに移動。
    /// </summary>
    public void GoToLobby()
    {
        if (playerStatus != null)
            playerStatus.ResetStatus();

        if (gameManager != null)
            gameManager.ExitDungeon();

        crystalInventory?.DepositCrystals();

        SceneManager.LoadScene("LobbyScene");
    }

    /// <summary>
    /// Titleシーンに移動。
    /// </summary>
    public void GoToTitle()
    {
        if (playerStatus != null)
            playerStatus.ResetStatus();

        if (gameManager != null)
            gameManager.ExitDungeon();

        crystalInventory?.DepositCrystals();

        SceneManager.LoadScene("TitleScene");
    }
}
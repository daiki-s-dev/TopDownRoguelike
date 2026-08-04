using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearSceneManager : MonoBehaviour
{
private GameManager gameManager;
private PlayerStatus playerStatus;
private PlayerCrystalInventory crystalInventory;

    void Start()
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

    // Lobbyシーンに移動
    public void GoToLobby()
    {
        if (playerStatus != null)
            playerStatus.ResetStatus();

        if (gameManager != null)
            gameManager.ExitDungeon();

        crystalInventory?.DepositCrystals();

        SceneManager.LoadScene("LobbyScene");
    }

    // Titleシーンに移動
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
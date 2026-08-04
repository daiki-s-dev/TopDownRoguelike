using UnityEngine;

/// <summary>
/// ダンジョンシーンで DontDestroyOnLoad の GameManager / PlayerStatus を参照するためのマネージャー。
/// </summary>
public class DungeonSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerStatus playerStatus;

    private void Start()
    {
        // DontDestroyOnLoad の GameManager と Player を参照
        gameManager = GameManager.Instance;
        playerStatus = PlayerStatus.Instance;

        if (gameManager == null)
            Debug.LogError("DungeonSceneManager: GameManager が見つかりません！");
        if (playerStatus == null)
            Debug.LogError("DungeonSceneManager: PlayerStatus が見つかりません！");
    }
}
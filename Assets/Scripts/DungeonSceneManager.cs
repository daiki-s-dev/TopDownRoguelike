using UnityEngine;

public class DungeonSceneManager : MonoBehaviour
{
private GameManager gameManager;
private PlayerStatus playerStatus;

    void Start()
    {
        // DontDestroyOnLoad ‚Ì GameManager ‚Æ Player ‚ğQÆ
        gameManager = GameManager.Instance;
        playerStatus = PlayerStatus.Instance;

        if (gameManager == null)
            Debug.LogError("DungeonSceneManager: GameManager ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñI");
        if (playerStatus == null)
            Debug.LogError("DungeonSceneManager: PlayerStatus ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñI");
    }



}
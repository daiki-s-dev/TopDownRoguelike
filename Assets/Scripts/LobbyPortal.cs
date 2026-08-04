using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyPortal : MonoBehaviour
{
    [Header("移動先のシーン名")]
    public string sceneToLoad = "DungeonScene"; // 遷移先のシーン名

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance?.PlaySE(SEType.PortalEnter);

            // ? タイマー開始
            TimeManager.Instance.StartTimer();

            // シーン遷移
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

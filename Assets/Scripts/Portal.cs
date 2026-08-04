using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    bool isActivated;

    [SerializeField] string bossFloorSceneName = "BossFloorScene";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated) return;
        if (!collision.CompareTag("Player")) return;

        isActivated = true;

        AudioManager.Instance?.PlaySE(SEType.PortalEnter);

        GameManager gm = GameManager.Instance;

        if (gm.floor == gm.maxFloor)
        {
            // ★ 最終フロア → ボスフロアへ
            SceneManager.LoadScene(bossFloorSceneName);
        }
        else
        {
            // 通常進行
            gm.NextFloor();
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// フロア間を移動するポータル。
/// 最終フロアではボスフロアへ、それ以外では通常の次フロア進行を行う。
/// </summary>
public class Portal : MonoBehaviour
{
    [SerializeField] private string bossFloorSceneName = "BossFloorScene";

    private bool isActivated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated) return;
        if (!collision.CompareTag("Player")) return;

        isActivated = true;

        AudioManager.Instance?.PlaySE(SEType.PortalEnter);

        GameManager gm = GameManager.Instance;

        if (gm.floor == gm.maxFloor)
        {
            // 最終フロア → ボスフロアへ
            SceneManager.LoadScene(bossFloorSceneName);
        }
        else
        {
            // 通常進行
            gm.NextFloor();
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ボス撃破後に出現するクリアポータル。
/// プレイヤーが触れるとクリアシーンへ遷移する。
/// </summary>
public class BossClearPortal : MonoBehaviour
{
    [Header("遷移先シーン名")]
    [SerializeField] private string clearSceneName = "ClearScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SceneManager.LoadScene(clearSceneName);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム起動時に最初に実行されるエントリーポイント。
/// タイムスケールをリセットし、タイトルシーンへ遷移する。
/// </summary>
public class Bootstrap : MonoBehaviour
{
    private void Start()
    {
        // 念のため Time.timeScale をリセット
        Time.timeScale = 1f;

        // タイトルシーンに切り替え（シングルロード）
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}
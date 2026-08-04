using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        // 念のため Time.timeScale をリセット
        Time.timeScale = 1f;

        // タイトルシーンに切り替え（シングルロード）
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 主要シーンへの遷移をまとめて呼び出せるようにするシングルトン。
/// </summary>
public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadTitle() => SceneManager.LoadScene("TitleScene");
    public void LoadLobby() => SceneManager.LoadScene("LobbyScene");
    public void LoadDungeon() => SceneManager.LoadScene("DungeonScene");
    public void LoadClear() => SceneManager.LoadScene("ClearScene");
}
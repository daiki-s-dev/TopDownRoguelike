using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ポーズメニューの開閉と設定画面、シーン遷移時の警告表示を管理するシングルトン。
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; private set; }

    [Header("PauseメニューUI")]
    public GameObject pauseMenuUI;

    [Header("設定画面UI")]
    public GameObject settingsUI;

    [Header("警告UI（Dungeonのみ）")]
    public GameObject warningUI;
    private string nextSceneName = "";

    public static bool IsPaused = false;

    private PlayerCrystalInventory crystalInventory;

    #region Unity Lifecycle

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        crystalInventory = PlayerCrystalInventory.Instance;
        if (crystalInventory == null)
            Debug.LogWarning("PauseMenuManager: PlayerCrystalInventory が見つかりません！");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // インベントリが開いている場合はポーズメニューを開かない
            if (InventoryUIController.Instance != null && InventoryUIController.Instance.IsOpen)
                return;

            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }

    #endregion

    #region ポーズ制御

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsUI != null) settingsUI.SetActive(false);

        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        IsPaused = true;
    }

    #endregion

    #region ボタン処理

    public void OnLobbyButton()
    {
        TryChangeScene("LobbyScene");
    }

    public void OnTitleButton()
    {
        TryChangeScene("TitleScene");
    }

    public void OnSettingsButton()
    {
        if (settingsUI != null)
            settingsUI.SetActive(!settingsUI.activeSelf);
    }

    /// <summary>
    /// 設定画面を閉じる（メニューに戻るボタン用）。
    /// </summary>
    public void CloseSettings()
    {
        if (settingsUI != null)
            settingsUI.SetActive(false);
    }

    #endregion

    #region DungeonSceneなら警告を出す

    private void TryChangeScene(string scene)
    {
        string current = SceneManager.GetActiveScene().name;

        if (current == "DungeonScene")
        {
            // アイテム消失警告を表示
            if (warningUI != null)
            {
                nextSceneName = scene;
                warningUI.SetActive(true);
            }
        }
        else
        {
            ChangeScene(scene);
        }
    }

    /// <summary>
    /// 警告 → OK を押したとき呼ばれる。
    /// </summary>
    public void OnWarningOK()
    {
        warningUI.SetActive(false);
        ChangeScene(nextSceneName);
        crystalInventory?.ResetCurrentSession();
    }

    /// <summary>
    /// 警告 → Cancel を押したとき呼ばれる。
    /// </summary>
    public void OnWarningCancel()
    {
        warningUI.SetActive(false);
        nextSceneName = "";
    }

    private void ChangeScene(string scene)
    {
        Resume();  // 時間停止解除
        SceneManager.LoadScene(scene);
    }

    #endregion
}
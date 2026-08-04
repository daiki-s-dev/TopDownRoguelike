using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("現在のステージ情報")]
    public int stage = 1;
    public int floor = 1;
    public int maxFloor = 5;

    [Header("祝福関連")]
    public BlessingSelectUI blessingUI;
    public List<Blessing> allBlessings;

    [Header("ステージ名表示UI")]
    public StageNameUI stageNameUI;

    [Header("シーン名設定")]
    [SerializeField] private string lobbySceneName = "LobbyScene";
    [SerializeField] private string dungeonSceneName = "DungeonScene";
    [SerializeField] private string clearSceneName = "ClearScene";

    public float clearTime { get; private set; }
    public int crystalCount { get; private set; }

    // 再生直後かどうか
    private bool isFirstLaunch = true;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetStageName() => $"{stage}-{floor}";
    public void SetClearTime(float t) => clearTime = t;
    public void SetCrystalCount(int c) => crystalCount = c;

    //==============================
    // ダンジョン進行
    //==============================
    public void EnterDungeon()
    {
        
        stage = 1;
        floor = 1;

        SceneManager.LoadScene(dungeonSceneName);
    }

    public void NextFloor()
    {
        floor++;

        if (floor > maxFloor)
        {
            StageClear();
        }
        else
        {
            ShowBlessingSelection();
        }

        MiniMapUI miniMap = FindFirstObjectByType<MiniMapUI>();
        if (miniMap != null)
            miniMap.RefreshFloorText();
    }

    private void ShowBlessingSelection()
    {
        if (blessingUI == null || allBlessings.Count == 0)
        {
            ReloadScene();
            return;
        }

        blessingUI.ShowBlessings(GenerateRandomBlessings(3));
    }

    private List<Blessing> GenerateRandomBlessings(int count)
    {
        List<Blessing> selected = new List<Blessing>();
        HashSet<int> used = new HashSet<int>();

        while (selected.Count < Mathf.Min(count, allBlessings.Count))
        {
            int idx = Random.Range(0, allBlessings.Count);
            if (used.Add(idx))
                selected.Add(allBlessings[idx]);
        }
        return selected;
    }

    public void LoadNextFloor()
    {
        ReloadScene();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StageClear()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.StopTimer();
            SetClearTime(TimeManager.Instance.GetTime());
        }

        if (PlayerCrystalInventory.Instance != null)
        {
            SetCrystalCount(PlayerCrystalInventory.Instance.GetCurrentSessionCrystals());
        }

        SceneManager.LoadScene(clearSceneName);
    }

    //==============================
    // シーンロード時処理
    //==============================
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ▶ 再生直後は何もしない
        if (isFirstLaunch)
        {
            isFirstLaunch = false;
            return;
        }

        // ▶ クリアシーンは何もしない
        if (scene.name == clearSceneName)
            return;

        // ▶ ロビー
        if (scene.name == lobbySceneName)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject spawn = GameObject.Find("LobbySpawnPoint");

            if (player != null && spawn != null)
            {
                player.transform.position = spawn.transform.position;
                player.transform.rotation = spawn.transform.rotation;
            }

            stageNameUI?.ShowStageName("ロビー");
            return;
        }

        // ▶ ダンジョン
        if (scene.name == dungeonSceneName)
        {
            RoomGenerator rg = Object.FindFirstObjectByType<RoomGenerator>();
            if (rg != null)
                rg.TeleportPlayerToStart();

            stageNameUI?.ShowStageName(GetStageName());
            return;
        }

        // ▶ ボスフロア
        if (scene.name == "BossFloorScene")
        {
            BossFloorController bossFloor = Object.FindFirstObjectByType<BossFloorController>();
            if (bossFloor != null && bossFloor.playerSpawnPoint != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = bossFloor.playerSpawnPoint.position;
                    player.transform.rotation = bossFloor.playerSpawnPoint.rotation;
                }
            }

            stageNameUI?.ShowStageName("最奥の部屋");
            return;
        }
    }



    //==============================
    // ロビーへ戻る
    //==============================
    public void ExitDungeon()
    {
        

        if (PlayerStatus.Instance != null)
            PlayerStatus.Instance.ResetStatus();

        if (BlessingManager.Instance != null)
            BlessingManager.Instance.ClearBlessingUI();

        stage = 1;
        floor = 1;

        
    }
}

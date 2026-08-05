using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ダンジョンの部屋をランダム生成するマネージャー。
/// 必須部屋（分岐点）の配置後、行き止まり部屋で残数を埋め、
/// 最後にスタートから最も遠い部屋にゴール（ポータル）を設置する。
/// </summary>
public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance { get; private set; }

    [Header("部屋設定")]
    public int roomCount = 10;
    public int roomSize = 16;
    public GameObject goalPrefab; // ポータルプレハブをInspectorで設定

    [Header("部屋中身プレハブ")]
    public GameObject[] enemyRoomContents;
    public GameObject[] eventRoomContents;

    private Dictionary<Vector2Int, GameObject> spawnedRooms = new Dictionary<Vector2Int, GameObject>();
    public List<GameObject> roomPrefabs = new List<GameObject>();
    public Vector2Int portalPos = Vector2Int.zero;

    #region Unity Lifecycle

    private void Awake()
    {
        // シングルトン化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // シーンロード時に呼ばれるコールバック登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DungeonScene") // ダンジョンシーンのみ生成
        {
            // Resources/RoomTemplate フォルダからプレハブ読み込み
            GameObject[] loadedRooms = Resources.LoadAll<GameObject>("RoomTemplate");
            roomPrefabs.Clear();
            roomPrefabs.AddRange(loadedRooms);

            if (roomPrefabs.Count == 0)
            {
                Debug.LogError("RoomTemplateフォルダにプレハブが見つかりません。Resources/RoomTemplate を確認してください。");
                return;
            }

            spawnedRooms.Clear();

            GenerateDungeon();
            PopulateRooms();
            TeleportPlayerToStart();
        }
    }

    #region プレイヤー配置

    /// <summary>
    /// プレイヤーをスタート位置に移動する。
    /// </summary>
    public void TeleportPlayerToStart()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = Vector3.zero;
        }
    }

    #endregion

    #region ダンジョン生成

    private void GenerateDungeon()
    {
        Vector2Int startPos = Vector2Int.zero;
        SpawnRoom("RoomTemplate_top", startPos); // スタート部屋
        int generated = 1;

        // 必須部屋1（3 or 4方向）
        Vector2Int pos1 = startPos + Vector2Int.up * roomSize;
        GameObject room1 = SpawnMandatoryRoom(new int[] { 3, 4 }, pos1, "down");
        if (room1 != null) generated++;

        // 必須部屋2・3
        List<Vector2Int> room1Dirs = GetAvailableDirections(pos1, room1);
        if (room1Dirs.Count >= 2)
        {
            Vector2Int pos2 = pos1 + room1Dirs[0] * roomSize;
            int typeRoom1 = CountExits(room1);
            int typeRoom2 = (typeRoom1 == 4) ? 3 : 4;
            GameObject room2 = SpawnMandatoryRoom(new int[] { typeRoom2 }, pos2, OppositeDir(room1Dirs[0]));
            if (room2 != null) generated++;

            Vector2Int pos3 = pos1 + room1Dirs[1] * roomSize;
            GameObject room3 = SpawnMandatoryRoom(new int[] { 2 }, pos3, OppositeDir(room1Dirs[1]));
            if (room3 != null) generated++;

            if (room3 != null)
            {
                List<Vector2Int> room3Dirs = GetAvailableDirections(pos3, room3);
                if (room3Dirs.Count > 0)
                {
                    Vector2Int pos4 = pos3 + room3Dirs[0] * roomSize;
                    GameObject r4 = SpawnMandatoryRoom(new int[] { 3 }, pos4, OppositeDir(room3Dirs[0]));
                    if (r4 != null) generated++;
                }
            }
        }

        // 1方向の行き止まり部屋
        List<Vector2Int> openPositions = new List<Vector2Int>(spawnedRooms.Keys);
        int remaining = roomCount - generated;
        for (int i = 0; i < remaining; i++)
        {
            bool placed = false;
            foreach (var pos in openPositions)
            {
                if (!spawnedRooms.TryGetValue(pos, out GameObject currentRoom)) continue;
                List<Vector2Int> dirs = GetAvailableDirections(pos, currentRoom);
                if (dirs.Count == 0) continue;

                Vector2Int dir = dirs[0];
                Vector2Int newPos = pos + dir * roomSize;
                List<GameObject> candidates = GetRoomsByExits(1, OppositeDir(dir));
                if (candidates.Count == 0) continue;

                GameObject newRoomPrefab = candidates[Random.Range(0, candidates.Count)];
                GameObject spawned = SpawnRoom(newRoomPrefab.name, newPos);
                if (spawned != null)
                {
                    openPositions.Add(newPos);
                    placed = true;
                    break;
                }
            }
            if (!placed) break;
        }

        // ゴール部屋を設置
        PlaceGoalRoom(startPos);
    }

    #endregion

    #region ゴール（ポータル）設置

    private void PlaceGoalRoom(Vector2Int startPos)
    {
        if (spawnedRooms.Count == 0 || goalPrefab == null) return;

        float maxDist = -1f;
        List<Vector2Int> farthestRooms = new List<Vector2Int>();

        foreach (var kvp in spawnedRooms)
        {
            float dist = Vector2Int.Distance(startPos, kvp.Key);
            if (dist > maxDist)
            {
                maxDist = dist;
                farthestRooms.Clear();
                farthestRooms.Add(kvp.Key);
            }
            else if (Mathf.Approximately(dist, maxDist))
            {
                farthestRooms.Add(kvp.Key);
            }
        }

        portalPos = farthestRooms[Random.Range(0, farthestRooms.Count)];
        Vector3 spawnPos = new Vector3(portalPos.x, portalPos.y, 0f);

        GameObject portal = Instantiate(goalPrefab, spawnPos, Quaternion.identity);
        portal.name = "Portal";
    }

    #endregion

    #region 部屋中身生成

    private void PopulateRooms()
    {
        Vector2Int startPos = Vector2Int.zero;
        List<GameObject> normalRooms = new List<GameObject>();
        foreach (var kvp in spawnedRooms)
        {
            if (kvp.Key == startPos || kvp.Key == portalPos) continue;
            normalRooms.Add(kvp.Value);
        }

        int enemyCount = Mathf.Min(5, normalRooms.Count);
        List<GameObject> enemyRooms = new List<GameObject>();
        List<GameObject> eventRooms = new List<GameObject>(normalRooms);

        for (int i = 0; i < enemyCount; i++)
        {
            if (eventRooms.Count == 0) break;
            int idx = Random.Range(0, eventRooms.Count);
            enemyRooms.Add(eventRooms[idx]);
            eventRooms.RemoveAt(idx);
        }

        foreach (var room in enemyRooms)
        {
            SpawnRoomContent(room, enemyRoomContents);
            room.name += "_EnemyRoom";
        }

        foreach (var room in eventRooms)
        {
            SpawnRoomContent(room, eventRoomContents);
            room.name += "_EventRoom";
        }

        Debug.Log("部屋の中身生成完了");
    }

    private void SpawnRoomContent(GameObject room, GameObject[] contents)
    {
        if (contents.Length == 0) return;

        Transform anchor = room.transform.Find("ContentAnchor");
        Vector3 pos = (anchor != null) ? anchor.position : room.transform.position;

        int index = Random.Range(0, contents.Length);
        GameObject content = Instantiate(contents[index], pos, Quaternion.identity);
        content.transform.SetParent(room.transform);
    }

    #endregion

    #region 補助メソッド

    private GameObject SpawnMandatoryRoom(int[] exitOptions, Vector2Int pos, string requiredDir)
    {
        List<GameObject> candidates = new List<GameObject>();
        foreach (var exits in exitOptions)
            candidates.AddRange(GetRoomsByExits(exits, requiredDir));

        if (candidates.Count == 0) return null;

        GameObject prefab = candidates[Random.Range(0, candidates.Count)];
        return SpawnRoom(prefab.name, pos);
    }

    private GameObject SpawnRoom(string prefabName, Vector2Int pos)
    {
        if (spawnedRooms.ContainsKey(pos)) return spawnedRooms[pos];

        GameObject prefab = Resources.Load<GameObject>("RoomTemplate/" + prefabName);
        if (prefab == null) return null;

        GameObject room = Instantiate(prefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        room.name = prefabName;
        spawnedRooms[pos] = room;
        return room;
    }

    private int CountExits(GameObject room)
    {
        int count = 0;
        string name = room.name;
        if (name.Contains("top")) count++;
        if (name.Contains("down")) count++;
        if (name.Contains("left")) count++;
        if (name.Contains("right")) count++;
        return count;
    }

    private List<GameObject> GetRoomsByExits(int exitCount, string requiredDir = "")
    {
        List<GameObject> result = new List<GameObject>();
        foreach (var r in roomPrefabs)
        {
            if (!string.IsNullOrEmpty(requiredDir) && !r.name.Contains(requiredDir)) continue;
            if (CountExits(r) == exitCount) result.Add(r);
        }
        return result;
    }

    private string OppositeDir(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return "down";
        if (dir == Vector2Int.down) return "top";
        if (dir == Vector2Int.left) return "right";
        if (dir == Vector2Int.right) return "left";
        return "";
    }

    private string DirToString(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return "top";
        if (dir == Vector2Int.down) return "down";
        if (dir == Vector2Int.left) return "left";
        if (dir == Vector2Int.right) return "right";
        return "";
    }

    private List<Vector2Int> GetAvailableDirections(Vector2Int pos, GameObject room)
    {
        List<Vector2Int> dirs = new List<Vector2Int>();
        foreach (var dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
        {
            Vector2Int newPos = pos + dir * roomSize;
            if (!spawnedRooms.ContainsKey(newPos) && room.name.Contains(DirToString(dir)))
                dirs.Add(dir);
        }
        return dirs;
    }

    public Dictionary<Vector2Int, GameObject> GetSpawnedRooms()
    {
        return spawnedRooms;
    }

    #endregion
}
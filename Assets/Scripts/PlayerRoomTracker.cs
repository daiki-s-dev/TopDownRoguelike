using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRoomTracker : MonoBehaviour
{
    private MiniMapUI miniMap;
    private RoomGenerator roomGen;
    private Vector2Int currentRoom = new Vector2Int(int.MinValue, int.MinValue);
    private int halfRoomSize = 8; // 中心からの切り替え距離

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        miniMap = FindFirstObjectByType<MiniMapUI>();
        roomGen = RoomGenerator.Instance;

        // シーンロード直後に最初の部屋を判定して枠表示
        if (miniMap != null && roomGen != null)
        {
            Vector3 pos = transform.position;

            int gridX = Mathf.RoundToInt(pos.x / roomGen.roomSize) * roomGen.roomSize;
            int gridY = Mathf.RoundToInt(pos.y / roomGen.roomSize) * roomGen.roomSize;
            currentRoom = new Vector2Int(gridX, gridY);

            if (roomGen.GetSpawnedRooms().ContainsKey(currentRoom))
            {
                miniMap.HighlightRoom(currentRoom);
            }
        }
    }

    void Update()
    {
        if (miniMap == null || roomGen == null) return;

        Vector3 pos = transform.position;

        // 中心座標に最も近い部屋
        int gridX = Mathf.RoundToInt(pos.x / roomGen.roomSize) * roomGen.roomSize;
        int gridY = Mathf.RoundToInt(pos.y / roomGen.roomSize) * roomGen.roomSize;
        Vector2Int targetRoom = new Vector2Int(gridX, gridY);

        // 現在の部屋から ±halfRoomSize 内なら切り替えなし
        float dx = Mathf.Abs(pos.x - currentRoom.x);
        float dy = Mathf.Abs(pos.y - currentRoom.y);

        if (dx < halfRoomSize && dy < halfRoomSize)
            return;

        // 部屋が変わったら更新
        if (targetRoom != currentRoom)
        {
            currentRoom = targetRoom;

            if (roomGen.GetSpawnedRooms().ContainsKey(currentRoom))
            {
                miniMap.HighlightRoom(currentRoom);
            }
        }
    }
}

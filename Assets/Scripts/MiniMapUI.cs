using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ダンジョン内のミニマップを表示するUI。
/// 各部屋のアイコン配置・訪問済み表示・現在地ハイライトを管理する。
/// </summary>
public class MiniMapUI : MonoBehaviour
{
    [Header("UI設定")]
    public GameObject miniMapPanel;
    public RectTransform mapContainer;
    public GameObject roomIconPrefab;

    [Header("部屋タイプのスプライト")]
    public Sprite startSprite;
    public Sprite goalSprite;
    public Sprite enemySprite;
    public Sprite eventSprite;

    [Header("色設定（背景用)")]
    public Color startColor = Color.green;
    public Color goalColor = Color.blue;
    public Color enemyColor = Color.red;
    public Color eventColor = Color.yellow;

    [Header("表示設定")]
    public float hiddenAlpha = 0.3f;
    public float visibleAlpha = 1f;

    [Header("フロア表示")]
    public TextMeshProUGUI floorText;
    [SerializeField] private string dungeonSceneName = "DungeonScene";

    private Dictionary<Vector2Int, GameObject> iconDict = new();
    private const int iconSize = 64;

    #region Unity Lifecycle

    private void Start()
    {
        // ダンジョンシーン以外ではミニマップを使わない
        if (SceneManager.GetActiveScene().name != dungeonSceneName)
        {
            gameObject.SetActive(false);
            return;
        }

        UpdateFloorText();
        GenerateMiniMap();
    }

    private void Update()
    {
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        bool pauseOpen = PauseMenuManager.IsPaused;

        if (miniMapPanel != null)
        {
            miniMapPanel.SetActive(!inventoryOpen && !pauseOpen);
        }
    }

    #endregion

    #region ミニマップ生成

    public void GenerateMiniMap()
    {
        iconDict.Clear();

        var rooms = RoomGenerator.Instance.GetSpawnedRooms();
        Vector2Int startPos = Vector2Int.zero;
        Vector2Int goalPos = RoomGenerator.Instance.portalPos;

        // マップ中心計算
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var kvp in rooms)
        {
            Vector2Int p = kvp.Key;
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }

        Vector2 mapCenter = new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);

        // アイコン生成
        foreach (var kvp in rooms)
        {
            Vector2Int pos = kvp.Key;
            GameObject icon = Instantiate(roomIconPrefab, mapContainer);
            iconDict[pos] = icon;

            Vector2 centeredPos = (new Vector2(pos.x, pos.y) - mapCenter);
            Vector2 miniPos = centeredPos / RoomGenerator.Instance.roomSize * iconSize;
            icon.GetComponent<RectTransform>().anchoredPosition = miniPos;

            // 背景色
            Image bgImage = icon.GetComponent<Image>();
            if (pos == startPos) bgImage.color = startColor;
            else if (pos == goalPos) bgImage.color = goalColor;
            else if (kvp.Value.name.Contains("_EnemyRoom")) bgImage.color = enemyColor;
            else if (kvp.Value.name.Contains("_EventRoom")) bgImage.color = eventColor;
            else bgImage.color = Color.black;

            // 中アイコン
            Image iconImg = icon.transform.Find("Icon")?.GetComponent<Image>();
            if (iconImg != null)
            {
                if (pos == startPos) iconImg.sprite = startSprite;
                else if (pos == goalPos) iconImg.sprite = goalSprite;
                else if (kvp.Value.name.Contains("_EnemyRoom")) iconImg.sprite = enemySprite;
                else if (kvp.Value.name.Contains("_EventRoom")) iconImg.sprite = eventSprite;

                iconImg.enabled = false;
            }

            // 枠は初期非表示
            Transform border = icon.transform.Find("Border");
            if (border != null) border.gameObject.SetActive(false);

            SetRoomVisited(icon, false);
        }

        // 開始部屋を訪問済みに
        if (iconDict.ContainsKey(startPos))
        {
            SetRoomVisited(iconDict[startPos], true);
            HighlightRoom(startPos);
        }
    }

    #endregion

    #region 現在部屋ハイライト

    public void HighlightRoom(Vector2Int roomPos)
    {
        foreach (var icon in iconDict.Values)
        {
            Transform border = icon.transform.Find("Border");
            if (border != null) border.gameObject.SetActive(false);
        }

        if (iconDict.ContainsKey(roomPos))
        {
            GameObject icon = iconDict[roomPos];

            Transform border = icon.transform.Find("Border");
            if (border != null) border.gameObject.SetActive(true);

            SetRoomVisited(icon, true);
        }
    }

    private void SetRoomVisited(GameObject icon, bool visited)
    {
        Image iconImg = icon.transform.Find("Icon")?.GetComponent<Image>();
        if (iconImg != null)
        {
            iconImg.enabled = visited;
            if (visited)
                iconImg.color = new Color(1f, 1f, 1f, visibleAlpha);
        }

        Image bg = icon.GetComponent<Image>();
        if (bg != null)
        {
            Color c = bg.color;
            c.a = visited ? visibleAlpha : hiddenAlpha;
            bg.color = c;
        }
    }

    #endregion

    #region フロア表示

    private void UpdateFloorText()
    {
        if (floorText == null || GameManager.Instance == null) return;

        floorText.text = $"{GameManager.Instance.stage}-{GameManager.Instance.floor}";
    }

    /// <summary>
    /// GameManager から呼ぶ用（次フロア）。
    /// </summary>
    public void RefreshFloorText()
    {
        UpdateFloorText();
    }

    #endregion
}
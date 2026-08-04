using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    [Header("カーソル設定")]
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        if (cursorTexture == null)
        {
            Debug.LogWarning("MouseCursorManager: カーソル画像が設定されていません");
            return;
        }

        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
    }
}

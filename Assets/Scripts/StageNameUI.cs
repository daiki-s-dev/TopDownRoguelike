using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// フロア移動時などにステージ名を一時的に表示するUI。
/// 表示後、一定時間でフェードアウトする。
/// </summary>
public class StageNameUI : MonoBehaviour
{
    [Header("表示テキスト")]
    public TextMeshProUGUI stageText;

    [Header("表示時間")]
    public float displayTime = 1.5f;
    public float fadeTime = 0.5f;

    private CanvasGroup canvasGroup;
    private Coroutine currentRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        // 最初は非表示（Activeは維持）
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void Update()
    {
        // インベントリ or ポーズ中なら非表示
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        bool pauseOpen = PauseMenuManager.IsPaused;

        bool hideUI = inventoryOpen || pauseOpen;

        // 開いた瞬間に即消す
        if (hideUI && canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha = 0f;
        }
    }

    public void ShowStageName(string stageName)
    {
        // インベントリ or ポーズ中なら表示しない
        bool inventoryOpen =
            InventoryUIController.Instance != null &&
            InventoryUIController.Instance.IsOpen;

        bool pauseOpen = PauseMenuManager.IsPaused;

        if (inventoryOpen || pauseOpen)
            return;

        stageText.text = stageName;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        // 表示
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(displayTime);

        // フェードアウト
        float t = 0f;
        while (t < fadeTime)
        {
            // フェード中にインベントリ or ポーズが開いたら即終了
            bool inventoryOpen =
                InventoryUIController.Instance != null &&
                InventoryUIController.Instance.IsOpen;

            bool pauseOpen = PauseMenuManager.IsPaused;

            if (inventoryOpen || pauseOpen)
            {
                canvasGroup.alpha = 0f;
                yield break;
            }

            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
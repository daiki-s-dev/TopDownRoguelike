using System.Collections;
using UnityEngine;

/// <summary>
/// MP不足時の警告表示を管理するシングルトン。
/// </summary>
public class MPWarningUI : MonoBehaviour
{
    public static MPWarningUI Instance;
    public GameObject textObj;

    private void Awake()
    {
        // 二重生成防止
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーン跨ぎ対応

        if (textObj != null)
            textObj.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void ShowNotEnoughMP()
    {
        Show();
    }

    public void Show()
    {
        // Destroy 済み対策
        if (this == null || gameObject == null)
            return;

        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        if (textObj == null) yield break;

        textObj.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        textObj.SetActive(false);
    }
}
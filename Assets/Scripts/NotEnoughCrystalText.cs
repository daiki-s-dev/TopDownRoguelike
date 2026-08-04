using System.Collections;
using UnityEngine;

/// <summary>
/// 魔石不足時に一定時間だけ表示される警告テキスト。
/// </summary>
public class NotEnoughCrystalText : MonoBehaviour
{
    [SerializeField] private float displayTime = 1.5f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        gameObject.SetActive(true);
        hideCoroutine = StartCoroutine(HideAfterTime());
    }

    private IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        gameObject.SetActive(false);
        hideCoroutine = null;
    }
}
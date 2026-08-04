using UnityEngine;
using System.Collections;

public class NotEnoughCrystalText : MonoBehaviour
{
    [SerializeField] float displayTime = 1.5f;

    Coroutine hideCoroutine;

    void Awake()
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

    IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        gameObject.SetActive(false);
        hideCoroutine = null;
    }
}

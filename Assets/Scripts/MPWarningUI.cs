using UnityEngine;
using System.Collections;

public class MPWarningUI : MonoBehaviour
{
    public static MPWarningUI Instance;
    public GameObject textObj;

    void Awake()
    {
        // Åö ìÒèdê∂ê¨ñhé~
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Åö ÉVÅ[Éìå◊Ç¨ëŒâû

        if (textObj != null)
            textObj.SetActive(false);
    }

    public void ShowNotEnoughMP()
    {
        Show();
    }

    public void Show()
    {
        // Åö Destroy çœÇ›ëŒçÙ
        if (this == null || gameObject == null)
            return;

        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        if (textObj == null) yield break;

        textObj.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        textObj.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}

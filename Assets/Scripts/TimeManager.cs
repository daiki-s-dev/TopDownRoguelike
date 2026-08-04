using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    // ★★★ リザルトUI用：経過時間取得 ★★★
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public float GetTime()
    {
        return elapsedTime;
    }
}

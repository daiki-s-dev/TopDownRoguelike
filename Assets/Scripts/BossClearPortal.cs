using UnityEngine;
using UnityEngine.SceneManagement;

public class BossClearPortal : MonoBehaviour
{
    [Header("‘JˆÚæƒV[ƒ“–¼")]
    [SerializeField] private string clearSceneName = "ClearScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SceneManager.LoadScene(clearSceneName);
    }
}

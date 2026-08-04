using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーを滑らかに追従するカメラ。
/// シーンが切り替わるたびに追従対象を再取得する。
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("追従設定")]
    public Transform target;   // 追従対象（プレイヤー）
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    #region Unity Lifecycle

    private void Awake()
    {
        // シーン切り替え時のコールバック登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }

    #endregion

    /// <summary>
    /// シーンロード時にプレイヤーを再取得する。
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }
}
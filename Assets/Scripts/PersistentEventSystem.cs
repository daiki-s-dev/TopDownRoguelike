using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// シーンをまたいでも破棄されない EventSystem を維持するためのシングルトン。
/// </summary>
public class PersistentEventSystem : MonoBehaviour
{
    private static PersistentEventSystem instance;

    private void Awake()
    {
        // すでに存在する場合は自分を破棄
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // シングルトン化
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
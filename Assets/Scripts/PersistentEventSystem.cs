using UnityEngine;
using UnityEngine.EventSystems;

public class PersistentEventSystem : MonoBehaviour
{
    private static PersistentEventSystem instance;

    void Awake()
    {
        // ‚·‚Å‚É‘¶İ‚·‚éê‡‚Í©•ª‚ğ”jŠü
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // ƒVƒ“ƒOƒ‹ƒgƒ“‰»
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource bgmSource;
    public AudioSource seSource;

    [System.Serializable]
    public class BGMSlot
    {
        public BGMType type;
        public AudioClip clip;
    }

    [System.Serializable]
    public class SESlot
    {
        public SEType type;
        public AudioClip clip;
    }

    [Header("BGM List")]
    public List<BGMSlot> bgms = new List<BGMSlot>();

    [Header("SE List")]
    public List<SESlot> ses = new List<SESlot>();

    private BGMType? currentBGM = null;

    [Header("音量設定")]
    [Range(0f,1f)] public float masterVolume = 1f;
    [Range(0f,1f)] public float bgmVolume = 1f;
    [Range(0f,1f)] public float seVolume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        // 起動時のシーンに対応
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        // AudioSource 初期反映
        UpdateVolumes();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // =====================
    // シーン読み込み時
    // =====================
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "TitleScene": PlayBGM(BGMType.Title); break;
            case "LobbyScene": PlayBGM(BGMType.Lobby); break;
            case "DungeonScene": PlayBGM(BGMType.Dungeon); break;
            case "BossFloorScene": PlayBGM(BGMType.Boss); break;
            case "ClearScene": StopBGM(); break;
        }
    }

    // =====================
    // 音量設定
    // =====================
    public void SetMasterVolume(float v) { masterVolume = Mathf.Clamp01(v); UpdateVolumes(); }
    public void SetBGMVolume(float v) { bgmVolume = Mathf.Clamp01(v); UpdateVolumes(); }
    public void SetSEVolume(float v) { seVolume = Mathf.Clamp01(v); UpdateVolumes(); }

    private void UpdateVolumes()
    {
        if (bgmSource != null) bgmSource.volume = bgmVolume * masterVolume;
        if (seSource != null) seSource.volume = seVolume * masterVolume;
    }

    // =====================
    // BGM
    // =====================
    public void PlayBGM(BGMType type, bool loop = true)
    {
        BGMSlot bgm = bgms.Find(x => x.type == type);
        if (bgm == null || bgm.clip == null) return;

        if (currentBGM == type && bgmSource.isPlaying) return;

        currentBGM = type;
        bgmSource.Stop();
        bgmSource.clip = bgm.clip;
        bgmSource.loop = loop;
        bgmSource.Play();
        UpdateVolumes();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
        currentBGM = null;
    }

    // =====================
    // SE
    // =====================
    public void PlaySE(SEType type, float volume = 1f)
    {
        SESlot se = ses.Find(x => x.type == type);
        if (se == null || se.clip == null) return;

        seSource.PlayOneShot(se.clip, volume * masterVolume * seVolume);
    }

    public void PlaySE(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        seSource.PlayOneShot(clip, volume * masterVolume * seVolume);
    }
}

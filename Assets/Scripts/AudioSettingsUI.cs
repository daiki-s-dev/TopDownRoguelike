using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// マスター/BGM/SE 音量を操作する設定画面のUI。
/// スライダー操作を AudioManager に反映し、パーセント表示も更新する。
/// </summary>
public class AudioSettingsUI : MonoBehaviour
{
    [Header("スライダー")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    [Header("パーセント表示用テキスト")]
    public TextMeshProUGUI masterText;
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI seText;

    private void Start()
    {
        if (AudioManager.Instance == null) return;

        InitializeSliders();
        RegisterSliderCallbacks();
    }

    /// <summary>
    /// 初期値セット（Notifyなしで1回だけ）＋パーセント表示の初期化。
    /// </summary>
    private void InitializeSliders()
    {
        if (masterSlider != null) masterSlider.SetValueWithoutNotify(AudioManager.Instance.masterVolume);
        if (bgmSlider != null) bgmSlider.SetValueWithoutNotify(AudioManager.Instance.bgmVolume);
        if (seSlider != null) seSlider.SetValueWithoutNotify(AudioManager.Instance.seVolume);

        UpdateText(masterText, masterSlider.value);
        UpdateText(bgmText, bgmSlider.value);
        UpdateText(seText, seSlider.value);
    }

    /// <summary>
    /// スライダーを触ったときのみ AudioManager に反映 + パーセント表示更新。
    /// </summary>
    private void RegisterSliderCallbacks()
    {
        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(v =>
            {
                AudioManager.Instance.SetMasterVolume(v);
                UpdateText(masterText, v);
            });

        if (bgmSlider != null)
            bgmSlider.onValueChanged.AddListener(v =>
            {
                AudioManager.Instance.SetBGMVolume(v);
                UpdateText(bgmText, v);
            });

        if (seSlider != null)
            seSlider.onValueChanged.AddListener(v =>
            {
                AudioManager.Instance.SetSEVolume(v);
                UpdateText(seText, v);
            });
    }

    private void UpdateText(TextMeshProUGUI text, float value)
    {
        if (text != null)
            text.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}
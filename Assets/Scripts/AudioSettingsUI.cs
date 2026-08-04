using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        // 初期値セット（Notifyなしで1回だけ）
        if (masterSlider != null) masterSlider.SetValueWithoutNotify(AudioManager.Instance.masterVolume);
        if (bgmSlider != null) bgmSlider.SetValueWithoutNotify(AudioManager.Instance.bgmVolume);
        if (seSlider != null) seSlider.SetValueWithoutNotify(AudioManager.Instance.seVolume);

        // パーセント表示も初期値で更新
        UpdateText(masterText, masterSlider.value);
        UpdateText(bgmText, bgmSlider.value);
        UpdateText(seText, seSlider.value);

        // スライダーを触ったときのみ AudioManager に反映 + パーセント表示更新
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

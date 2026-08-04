using UnityEngine;
using TMPro;

public class PlayerStatusUI : MonoBehaviour
{
    [Header("UI Text éQè∆")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI hpRegenText;
    public TextMeshProUGUI mpRegenText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI magicText;
    public TextMeshProUGUI critRateText;
    public TextMeshProUGUI critDamageText;

    private PlayerStatus status;

    void Start()
    {
        status = PlayerStatus.Instance;
    }

    void Update()
    {
        if (status == null) return;

        hpText.text = $"{status.currentHP} / {status.maxHP}";
        mpText.text = $"{status.currentMP} / {status.maxMP}";
        hpRegenText.text = $"{status.hpRegenRate:F1} / s";
        mpRegenText.text = $"{status.mpRegenRate:F1} / s";
        attackText.text = status.attack.ToString();
        magicText.text = status.magic.ToString();
        critRateText.text = $"{status.criticalRate * 100f:F1}%";
        critDamageText.text = $"{status.criticalDamage:F1}x";
    }
}

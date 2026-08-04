using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 合成ボタンの操作と合成結果メッセージの表示を担当する。
/// </summary>
public class MagicCircleSynthesisController : MonoBehaviour
{
    public MagicCircleSynthesis synthesis;
    public Button synthesizeButton;
    public TMP_Text messageText;

    private void Awake()
    {
        ResetMessage();

        if (synthesizeButton != null)
            synthesizeButton.onClick.AddListener(OnClickSynthesize);
    }

    private void OnClickSynthesize()
    {
        ResetMessage();

        SynthesisResult result = synthesis.TrySynthesize();

        switch (result)
        {
            case SynthesisResult.EmptySlot:
                messageText.text = "アイテムがセットされていません";
                break;

            case SynthesisResult.DifferentRarity:
                messageText.text = "レアリティが異なります";
                break;

            case SynthesisResult.CannotSynthesize:
                messageText.text = "これ以上合成できません";
                break;

            case SynthesisResult.Success:
                // UIは閉じられる
                break;
        }
    }

    public void ResetMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }
}
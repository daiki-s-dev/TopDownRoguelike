using UnityEngine;

/// <summary>
/// 魔法陣UIの閉じるボタンに紐付けるコンポーネント。
/// </summary>
public class MagicCircleCloseButton : MonoBehaviour
{
    public MagicCircleUIController magicCircleUI;

    public void OnClickClose()
    {
        magicCircleUI.Close();
    }
}
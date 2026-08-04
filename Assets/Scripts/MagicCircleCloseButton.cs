using UnityEngine;

public class MagicCircleCloseButton : MonoBehaviour
{
    public MagicCircleUIController magicCircleUI;

    public void OnClickClose()
    {
        magicCircleUI.Close();
    }
}

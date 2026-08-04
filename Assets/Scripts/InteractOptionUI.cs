// Assets/Scripts/InteractOptionUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractOptionUI : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Image background;

    // •Ö—˜ƒƒ\ƒbƒh
    public void SetText(string s)
    {
        if (label != null) label.text = s;
    }
}

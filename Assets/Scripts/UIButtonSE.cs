using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSE : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler
{
    [Header("SEê›íË")]
    public SEType hoverSE = SEType.ButtonHover;
    public SEType clickSE = SEType.ButtonClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance?.PlaySE(hoverSE);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance?.PlaySE(clickSE);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UIボタンにホバー音・クリック音を付与する。
/// </summary>
public class UIButtonSE : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler
{
    [Header("SE設定")]
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
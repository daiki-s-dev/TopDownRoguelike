using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PermanentBlessingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text costText;

    private PermanentBlessing blessing;
    private PermanentBlessingUIController uiController;

    public void SetData(PermanentBlessing blessing, PermanentBlessingUIController controller)
    {
        this.blessing = blessing;
        this.uiController = controller;

        icon.sprite = blessing.icon;
        nameText.text = blessing.blessingName;
        costText.text = blessing.cost.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiController.ShowDescription(blessing.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiController.ShowDescription("");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uiController.TryPurchase(blessing);
    }
}

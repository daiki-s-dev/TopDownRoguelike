using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PermanentBlessingUIController : MonoBehaviour
{
    [Header("ScriptableObjectリスト")]
    public List<PermanentBlessingSO> blessingSOList;

    [Header("UIパネル")]
    public GameObject uiRoot;

    [Header("魔石表示")]
    public TMP_Text crystalText;

    [Header("祝福ボタンプレハブ")]
    public GameObject blessingButtonPrefab;
    public Transform buttonParent;

    [Header("祝福説明欄")]
    public TMP_Text descriptionText;

    [Header("恒久祝福表示欄")]
    public Transform permanentBlessingParent;
    public GameObject permanentBlessingPrefab;

    [Header("魔石不足テキスト")]
    [SerializeField] private NotEnoughCrystalText notEnoughCrystalText;

    private List<GameObject> currentButtons = new List<GameObject>();
    private List<PermanentBlessing> availableBlessings = new List<PermanentBlessing>();

    void Start()
    {
        uiRoot.SetActive(false);

        // ScriptableObject → PermanentBlessing
        availableBlessings.Clear();
        foreach (var so in blessingSOList)
        {
            availableBlessings.Add(so.ToPermanentBlessing());
        }
    }

    public void OpenUI()
    {
        uiRoot.SetActive(true);
        UpdateUI();
    }

    public void CloseUI()
    {
        uiRoot.SetActive(false);
    }

    public void UpdateUI()
    {
        // 魔石表示
        crystalText.text = $"魔石: {PlayerCrystalInventory.Instance.TotalCrystals}";

        // 既存ボタン削除
        foreach (var btn in currentButtons)
            Destroy(btn);
        currentButtons.Clear();

        // ボタン生成
        foreach (var blessing in availableBlessings)
        {
            GameObject btnObj = Instantiate(blessingButtonPrefab, buttonParent);
            currentButtons.Add(btnObj);

            PermanentBlessingButton btn = btnObj.GetComponent<PermanentBlessingButton>();
            btn.SetData(blessing, this);
        }

        UpdatePermanentUI();
    }

    public void UpdatePermanentUI()
    {
        foreach (Transform t in permanentBlessingParent)
            Destroy(t.gameObject);

        if (PermanentBlessingManager.Instance == null) return;

        foreach (var b in PermanentBlessingManager.Instance.permanentBlessings)
        {
            GameObject obj = Instantiate(permanentBlessingPrefab, permanentBlessingParent);
            PermanentBlessingDisplay display = obj.GetComponent<PermanentBlessingDisplay>();
            display.SetData(b.blessing, b.count);
        }
    }

    public void ShowDescription(string text)
    {
        descriptionText.text = text;
    }

    //========================================
    // ■ 祝福購入処理
    //========================================
    public void TryPurchase(PermanentBlessing blessing)
    {
        if (PlayerCrystalInventory.Instance.ConsumeCrystal(blessing.cost))
        {
            PermanentBlessingManager.Instance.AddBlessing(blessing);
            UpdateUI();
        }
        else
        {
            // ★ 魔石不足表示
            if (notEnoughCrystalText != null)
            {
                notEnoughCrystalText.Show();
            }

            Debug.Log("魔石が足りません");
        }
    }

    // 戻るボタン
    public void OnBackButton()
    {
        CloseUI();
    }
}

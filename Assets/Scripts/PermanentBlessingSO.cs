using UnityEngine;

[CreateAssetMenu(fileName = "PermanentBlessingData", menuName = "Blessing/PermanentBlessing")]
public class PermanentBlessingSO : ScriptableObject
{
    [Header("基本情報")]
    public string blessingName;          // 祝福名
    public BlessingType type;            // 元のBlessingのタイプ
    public float value;                  // 効果値
    public bool isMultiplier;            // 乗算か加算か

    [Header("UI用")]
    public Sprite icon;                  // アイコン
    [TextArea] public string description;// 説明
    public int cost;                     // 魔石価格

    // このSOから PermanentBlessing を生成する関数
    public PermanentBlessing ToPermanentBlessing()
    {
        return new PermanentBlessing
        {
            blessingName = blessingName,
            type = type,
            value = value,
            isMultiplier = isMultiplier,
            icon = icon,
            description = description,
            cost = cost
        };
    }
}

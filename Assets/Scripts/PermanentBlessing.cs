using UnityEngine;

[System.Serializable]
public class PermanentBlessing
{
    public string blessingName;       // 祝福名
    public BlessingType type;         // 元のBlessingのタイプ
    public float value;               // 効果値
    public bool isMultiplier;         // 乗算か加算か

    public Sprite icon;               // UI用アイコン
    [TextArea] public string description;  // UI用説明
    public int cost;                  // 魔石価格

    public PermanentBlessing() { }

    public PermanentBlessing(Blessing baseBlessing, int cost, Sprite icon = null, string description = "")
    {
        this.blessingName = baseBlessing.blessingName;
        this.type = baseBlessing.type;
        this.value = baseBlessing.value;
        this.isMultiplier = baseBlessing.isMultiplier;
        this.cost = cost;
        this.icon = icon;
        this.description = description;
    }
}

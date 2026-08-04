/// <summary>
/// ダメージ計算結果（ダメージ量とクリティカル有無）を表す構造体。
/// </summary>
public struct DamageResult
{
    public int damage;
    public bool isCritical;

    public DamageResult(int damage, bool isCritical)
    {
        this.damage = damage;
        this.isCritical = isCritical;
    }
}
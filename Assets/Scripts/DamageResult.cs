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

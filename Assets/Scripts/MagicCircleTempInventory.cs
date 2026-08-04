using System.Collections.Generic;

/// <summary>
/// 魔法陣UIを開いている間だけ使う一時的な武器インベントリ。
/// プレイヤーの本インベントリのコピーとして扱う。
/// </summary>
public class MagicCircleTempInventory
{
    public List<WeaponData> weapons;

    public MagicCircleTempInventory(PlayerInventory source)
    {
        weapons = new List<WeaponData>(source.weapons);
    }

    public void Remove(WeaponData weapon)
    {
        weapons.Remove(weapon);
    }

    public void Add(WeaponData weapon)
    {
        weapons.Add(weapon);
    }
}
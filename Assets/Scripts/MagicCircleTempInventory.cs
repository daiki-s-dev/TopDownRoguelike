using System.Collections.Generic;

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

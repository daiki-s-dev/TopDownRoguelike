using UnityEngine;

/// <summary>
/// Resources フォルダのパス指定で武器を装備させる、汎用の武器マネージャー。
/// </summary>
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance; // インベントリ側から呼ぶために追加

    [Header("武器を持つ位置（WeaponPoint）")]
    public Transform weaponPoint;

    private GameObject currentWeapon;

    private void Awake()
    {
        Instance = this;
    }

    // ゲーム開始時に装備しないよう Start は使用しない
    // void Start()
    // {
    //     EquipWeapon("Weapons/Sword01Prefab");
    // }

    public void EquipWeapon(string weaponPath)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        GameObject weaponPrefab = Resources.Load<GameObject>(weaponPath);

        if (weaponPrefab != null)
        {
            currentWeapon = Instantiate(weaponPrefab, weaponPoint);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("武器が見つかりません: " + weaponPath);
        }
    }
}
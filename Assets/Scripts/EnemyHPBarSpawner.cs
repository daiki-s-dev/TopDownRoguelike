
using UnityEngine;

public class EnemyHPBarSpawner : MonoBehaviour
{
    public GameObject hpBarPrefab;

    private void Start()
    {
        EnemyBase enemy = GetComponent<EnemyBase>();
        if (enemy == null)
        {
            Debug.LogError("EnemyHPBarSpawner: EnemyBase が見つかりません");
            return;
        }

        if (hpBarPrefab == null)
        {
            Debug.LogError("HPバーPrefabが設定されていません");
            return;
        }

        // HPバー生成
        GameObject bar = Instantiate(hpBarPrefab, enemy.transform.position, Quaternion.identity);

        // 初期化
        bar.GetComponent<EnemyHPBarUI>().Init(enemy);
    }
}

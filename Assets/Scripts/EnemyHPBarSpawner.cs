using UnityEngine;

/// <summary>
/// 敵の出現時にHPバーUIを生成し、対応する EnemyBase に紐付ける。
/// </summary>
public class EnemyHPBarSpawner : MonoBehaviour
{
    [Header("HPバー")]
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
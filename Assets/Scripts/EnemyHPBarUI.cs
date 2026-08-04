using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵の頭上に表示されるHPバー。
/// 対象の敵のHP変化・死亡イベントを購読して表示を更新する。
/// </summary>
public class EnemyHPBarUI : MonoBehaviour
{
    [Header("表示設定")]
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    private EnemyBase enemy;
    private Camera cam;

    public void Init(EnemyBase target)
    {
        enemy = target;
        cam = Camera.main;

        UpdateHP(enemy.hp);

        enemy.onHpChanged += OnHpChanged;
        enemy.onEnemyDead += OnEnemyDead;
    }

    private void LateUpdate()
    {
        if (enemy == null) return;

        transform.position = enemy.transform.position + offset;

        if (cam != null)
            transform.rotation = cam.transform.rotation;
    }

    private void OnHpChanged(int currentHp, int maxHp)
    {
        UpdateHP(currentHp);
        // destroy しない！
    }

    private void UpdateHP(int hp)
    {
        Debug.Log($"HP Update: {hp}/{enemy.maxHp}");
        if (fillImage != null && enemy != null)
            fillImage.fillAmount = Mathf.Clamp01((float)hp / enemy.maxHp);
    }

    private void OnEnemyDead(EnemyBase e)
    {
        if (this != null)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.onHpChanged -= OnHpChanged;
            enemy.onEnemyDead -= OnEnemyDead;
        }
    }
}
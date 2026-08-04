using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBarUI : MonoBehaviour
{
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
        // Åö destroy ÇµÇ»Ç¢ÅI
    }

    void UpdateHP(int hp)
    {
        Debug.Log($"HP Update: {hp}/{enemy.maxHp}");
        if (fillImage != null && enemy != null)
            fillImage.fillAmount = Mathf.Clamp01((float)hp / enemy.maxHp);
    }

    void OnEnemyDead(EnemyBase e)
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

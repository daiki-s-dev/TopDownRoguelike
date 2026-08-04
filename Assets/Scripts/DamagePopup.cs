using TMPro;
using UnityEngine;

/// <summary>
/// ダメージ数値を表示するポップアップ。
/// 上に浮かびながら一定時間後に消える。
/// </summary>
public class DamagePopup : MonoBehaviour
{
    [Header("参照・設定")]
    public TMP_Text text;
    public float moveY = 1f;
    public float lifetime = 1f;
    public Vector3 offset = new Vector3(0, 2f, 0);

    private float timer = 0f;
    private Vector3 worldPos;

    public void Setup(int damage, bool isCritical, Vector3 spawnWorldPos)
    {
        worldPos = spawnWorldPos;

        if (text == null)
            text = GetComponent<TMP_Text>();

        text.text = damage.ToString();

        Debug.Log($"DamagePopup Setup called. Damage: {damage}, isCritical: {isCritical}");

        if (isCritical)
        {
            text.color = Color.red;
            text.fontSize = 50;
        }
        else
        {
            text.color = Color.white;
            text.fontSize = 36;
        }

        transform.position = worldPos + offset;
    }

    private void Update()
    {
        transform.position += Vector3.up * moveY * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);
    }
}
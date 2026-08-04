using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// HP/MP回復量を表示するポップアップ。
/// 上に浮かびながら一定時間後に消える。
/// </summary>
public class HealPopup : MonoBehaviour
{
    [Header("設定")]
    public TMP_Text text;
    public float floatSpeed = 1f;
    public float lifeTime = 1f;

    public void Setup(int value, HealType type, Vector3 worldPos)
    {
        transform.position = worldPos;

        text.text = "+" + value;

        // 色分け
        switch (type)
        {
            case HealType.HP:
                text.color = new Color(0.6f, 1f, 0.4f); // 黄緑
                break;
            case HealType.MP:
                text.color = Color.cyan; // 青
                break;
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float t = 0;
        while (t < lifeTime)
        {
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
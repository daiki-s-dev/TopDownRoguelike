using UnityEngine;
using TMPro;
using System.Collections;

public class HealPopup : MonoBehaviour
{
    public TMP_Text text;
    public float floatSpeed = 1f;
    public float lifeTime = 1f;

    public void Setup(int value, HealType type, Vector3 worldPos)
    {
        transform.position = worldPos;

        text.text = "+" + value;

        // êFï™ÇØ
        switch (type)
        {
            case HealType.HP:
                text.color = new Color(0.6f, 1f, 0.4f); // â©óŒ
                break;
            case HealType.MP:
                text.color = Color.cyan; // ê¬
                break;
        }

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
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

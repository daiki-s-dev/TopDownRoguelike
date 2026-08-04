using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    public GameObject zanzouPrefab;              // 斬撃エフェクトのPrefab
    public Transform slashEffectSpawnPoint;      // エフェクト出現位置（WeaponPoint の子）
    public Transform playerTransform;            // 01Knight_1 または Player の Transform

    // 斬撃エフェクトを再生する関数
    public void PlayZanzou()
    {
        if (zanzouPrefab != null && slashEffectSpawnPoint != null)
        {
            // エフェクト生成
            GameObject effect = Instantiate(
                zanzouPrefab,
                slashEffectSpawnPoint.position,
                slashEffectSpawnPoint.rotation
            );

            // プレイヤーが左を向いている場合はエフェクトの X スケールを反転
            if (playerTransform != null && playerTransform.localScale.x < 0)
            {
                Vector3 scale = effect.transform.localScale;
                scale.x *= -1f;
                effect.transform.localScale = scale;
            }
        }
    }
}

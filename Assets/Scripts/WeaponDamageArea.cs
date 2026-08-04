using UnityEngine;
using System;

public class WeaponDamageArea : MonoBehaviour
{
    [Header("UŒ‚‘ÎÛ‚Æ‚È‚éƒ^ƒO")]
    public string targetTag = "Enemy";

    public Action<Collider2D> OnHitEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
            OnHitEnemy?.Invoke(other);
    }
}

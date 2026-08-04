using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;
    private PlayerWeaponEquip weaponEquip;

    [Header("回復ポップアップ")]
    public HealPopupSpawner healPopupSpawner;

    //========================================
    // ■ 元のステータス（祝福で加算する前の値）
    //========================================
    private int baseAttack;
    private int baseMaxHP;
    private int baseMaxMP;
    private int baseMagic;
    private float baseHpRegenRate;
    private float baseMpRegenRate;
    private float baseCriticalRate;
    private float baseCriticalDamage;

    //========================================
    // ■ プレイヤー基本ステータス
    //========================================

    [Header("基本ステータス")]
    public int maxHP = 100;
    public int currentHP = 100;

    public int maxMP = 50;
    public int currentMP = 50;

    [Header("自動回復")]
    public float hpRegenRate = 0f;
    public float mpRegenRate = 0f;

    private float hpRegenTimer = 0f;
    private float mpRegenTimer = 0f;

    [Header("攻撃関連")]
    public int attack = 10;
    public int magic = 10;

    [Range(0f, 1f)]
    public float criticalRate = 0.1f;
    public float criticalDamage = 1.5f;

    //========================================
    // ■ 演出（点滅 / ノックバック / 死亡）
    //========================================

    [Header("被ダメージ演出設定")]
    public float flashDuration = 0.1f;
    public float knockbackForce = 5f;
    public float knockbackTime = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerAnimation playerAnimation;

    private bool isKnockedBack = false;
    private bool isDead = false;

    //========================================
    // ■ 祝福（バフ）管理
    //========================================

    [System.Serializable]
    public class ActiveBlessing
    {
        public Blessing blessing;
        public int stackCount = 1;   // 同じ祝福を重ねた回数
    }

    public List<ActiveBlessing> activeBlessings = new List<ActiveBlessing>();

    //========================================
    // ■ Unity標準処理
    //========================================

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        weaponEquip = GetComponent<PlayerWeaponEquip>();

        if (playerAnimation == null)
            Debug.LogError("PlayerAnimation が見つかりません。");

        // 元値を保持
        baseAttack = attack;
        baseMaxHP = maxHP;
        baseMaxMP = maxMP;
        baseMagic = magic;
        baseHpRegenRate = hpRegenRate;
        baseMpRegenRate = mpRegenRate;
        baseCriticalRate = criticalRate;
        baseCriticalDamage = criticalDamage;
    }

    void Update()
    {
        AutoRegen();
    }

    //========================================
    // ■ 祝福取得
    //========================================

    public void ApplyBlessing(Blessing newBlessing)
    {
        var existing = activeBlessings.Find(b => b.blessing.type == newBlessing.type);
        if (existing != null)
            existing.stackCount++;
        else
            activeBlessings.Add(new ActiveBlessing { blessing = newBlessing, stackCount = 1 });

        // 最大値や攻撃力を再計算（currentHP/MPは維持）
        RecalculateStats();

        Debug.Log("ApplyBlessing called: " + newBlessing.blessingName);

        if (BlessingManager.Instance != null)
        {
            Debug.Log("Calling UpdateBlessingUI...");
            BlessingManager.Instance.UpdateBlessingUI(activeBlessings);
        }
        else
        {
            Debug.LogWarning("BlessingManager.Instance is null!");
        }

        Debug.Log($"祝福取得: {newBlessing.blessingName} x{(existing != null ? existing.stackCount : 1)}");
        Debug.Log("UpdateBlessingUI called. Count: " + activeBlessings.Count);
    }

    public void RecalculateStats()
    {
        attack = baseAttack;
        maxHP = baseMaxHP;
        maxMP = baseMaxMP;
        magic = baseMagic;
        hpRegenRate = baseHpRegenRate;
        mpRegenRate = baseMpRegenRate;
        criticalRate = baseCriticalRate;
        criticalDamage = baseCriticalDamage;

        // ★ 0. 武器補正（最優先）
        if (weaponEquip != null && weaponEquip.EquippedWeapon != null)
        {
            ApplyWeaponBonus(weaponEquip.EquippedWeapon);
        }

        // 1. ダンジョン内祝福
        foreach (var ab in activeBlessings)
        {
            ApplyBlessingEffect(ab.blessing, ab.stackCount);
        }

        // 2. 恒久祝福
        if (PermanentBlessingManager.Instance != null)
        {
            foreach (var pb in PermanentBlessingManager.Instance.permanentBlessings)
            {
                ApplyPermanentBlessingEffect(pb.blessing, pb.count);
            }
        }
    }

    private void ApplyBlessingEffect(Blessing b, int stacks)
    {
        float valueToApply = b.isMultiplier ? Mathf.Pow(b.value, stacks) : b.value * stacks;

        switch (b.type)
        {
            case BlessingType.AttackUp:
                attack = b.isMultiplier ? Mathf.RoundToInt(attack * valueToApply) : attack + Mathf.RoundToInt(valueToApply);
                break;
            case BlessingType.MaxHPUp:
                maxHP = b.isMultiplier ? Mathf.RoundToInt(maxHP * valueToApply) : maxHP + Mathf.RoundToInt(valueToApply);
                break;
            case BlessingType.MaxMPUp:
                maxMP = b.isMultiplier ? Mathf.RoundToInt(maxMP * valueToApply) : maxMP + Mathf.RoundToInt(valueToApply);
                break;
            case BlessingType.HPRegenUp:
                hpRegenRate = b.isMultiplier ? hpRegenRate * valueToApply : hpRegenRate + valueToApply;
                break;
            case BlessingType.MPRegenUp:
                mpRegenRate = b.isMultiplier ? mpRegenRate * valueToApply : mpRegenRate + valueToApply;
                break;
            case BlessingType.MagicUp:
                magic = b.isMultiplier ? Mathf.RoundToInt(magic * valueToApply) : magic + Mathf.RoundToInt(valueToApply);
                break;
            case BlessingType.CriticalRateUp:
                criticalRate = b.isMultiplier ? criticalRate * valueToApply : criticalRate + valueToApply;
                break;
            case BlessingType.CriticalDamageUp:
                criticalDamage = b.isMultiplier ? criticalDamage * valueToApply : criticalDamage + valueToApply;
                break;
            case BlessingType.PotionBoost:
            case BlessingType.CristalDropRateUp:
                // ステータス値に直接関係しないものはここで無視
                break;
            default:
                Debug.LogWarning("未実装の祝福タイプ: " + b.type);
                break;
        }
    }


    private void ApplyPermanentBlessingEffect(PermanentBlessing b, int count)
    {
        float valueToApply = b.isMultiplier ? Mathf.Pow(b.value, count) : b.value * count;

        switch (b.type)
        {
            case BlessingType.AttackUp:
                attack = b.isMultiplier ? Mathf.RoundToInt(attack * valueToApply) : attack + Mathf.RoundToInt(valueToApply);
                break;
            case BlessingType.MaxHPUp:
                maxHP = b.isMultiplier ? Mathf.RoundToInt(maxHP * valueToApply) : maxHP + Mathf.RoundToInt(valueToApply);
                break;
            case BlessingType.MaxMPUp:
                maxMP = b.isMultiplier ? Mathf.RoundToInt(maxMP * valueToApply) : maxMP + Mathf.RoundToInt(valueToApply);
                break;
            case BlessingType.HPRegenUp:   // ★ 追加
                hpRegenRate = b.isMultiplier ? hpRegenRate * valueToApply : hpRegenRate + valueToApply;
                break;
            case BlessingType.MPRegenUp:   // ★ 追加
                mpRegenRate = b.isMultiplier ? mpRegenRate * valueToApply : mpRegenRate + valueToApply;
                break;
            case BlessingType.MagicUp:
                magic = b.isMultiplier ? Mathf.RoundToInt(magic * valueToApply) : magic + Mathf.RoundToInt(valueToApply);
                break;
            case BlessingType.CriticalRateUp:
                criticalRate = b.isMultiplier ? criticalRate * valueToApply : criticalRate + valueToApply;
                break;
            case BlessingType.CriticalDamageUp:
                criticalDamage = b.isMultiplier ? criticalDamage * valueToApply : criticalDamage + valueToApply;
                break;
            // 他のタイプも必要に応じて追加
        }
    }


    // ★ 指定のタイプの倍率を取得
    public float GetMultiplier(BlessingType type)
    {
        float multiplier = 1f;
        foreach (var ab in activeBlessings)
        {
            if (ab.blessing.type == type)
            {
                int stacks = ab.stackCount;
                Blessing b = ab.blessing;
                multiplier *= b.isMultiplier ? Mathf.Pow(b.value, stacks) : 1f + b.value * stacks;
            }
        }
        return multiplier;
    }

    public int GetPotionHealAmount(int baseAmount)
    {
        return Mathf.RoundToInt(baseAmount * GetMultiplier(BlessingType.PotionBoost));
    }

    public float GetDropRateMultiplier()
    {
        return GetMultiplier(BlessingType.CristalDropRateUp);
    }

    //========================================
    // ■ 自動回復処理
    //========================================

    private void AutoRegen()
    {
        if (hpRegenRate > 0 && currentHP < maxHP)
        {
            hpRegenTimer += Time.deltaTime;
            if (hpRegenTimer >= 1f)
            {
                currentHP = Mathf.Min(maxHP, currentHP + Mathf.RoundToInt(hpRegenRate));
                hpRegenTimer = 0f;
            }
        }

        if (mpRegenRate > 0 && currentMP < maxMP)
        {
            mpRegenTimer += Time.deltaTime;
            if (mpRegenTimer >= 1f)
            {
                currentMP = Mathf.Min(maxMP, currentMP + Mathf.RoundToInt(mpRegenRate));
                mpRegenTimer = 0f;
            }
        }
    }

    //========================================
    // ■ ダメージ処理
    //========================================

    public void TakeDamage(int damage, Vector2 hitSourcePosition)
    {
        if (isDead) return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        AudioManager.Instance?.PlaySE(SEType.PlayerDamage);

        Debug.Log($"プレイヤーが {damage} ダメージを受けた！ 残りHP: {currentHP}");

        DamagePopupSpawner popupSpawner = GetComponent<DamagePopupSpawner>();
        if (popupSpawner != null)
            popupSpawner.CreatePopup(damage, false);

        Vector2 knockDir = (transform.position - (Vector3)hitSourcePosition).normalized;
        StartCoroutine(DamageFlash());
        StartCoroutine(KnockbackRoutine(knockDir));

        if (playerAnimation != null && currentHP > 0)
            playerAnimation.TakeDamage(knockDir);

        if (currentHP <= 0)
            Die(knockDir);
    }

    private IEnumerator DamageFlash()
    {
        if (sr == null) yield break;

        Color original = sr.color;
        sr.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        sr.color = original;
    }

    private IEnumerator KnockbackRoutine(Vector2 direction)
    {
        if (rb == null) yield break;

        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;

        float massFactor = Mathf.Clamp(1f / Mathf.Sqrt(rb.mass), 0.2f, 2f);
        rb.AddForce(direction * knockbackForce * massFactor, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackTime);

        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    private void Die(Vector2 hitDirection)
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("プレイヤー死亡！");

        if (playerAnimation != null)
            playerAnimation.PlayDeathAnimation(hitDirection);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        GameOverUIController ui = FindFirstObjectByType<GameOverUIController>();
        if (ui != null)
            ui.ShowGameOver();
    }

    //========================================
    // ■ ユーティリティ
    //========================================

    public bool IsKnockedBack() => isKnockedBack;

    public bool UseMP(int value)
    {
        if (currentMP < value) return false;

        currentMP -= value;
        return true;
    }

    //========================================
    // ■ 敵に与えるダメージ
    //========================================

    public void AttackEnemy(EnemyBase enemy)
    {
        if (enemy == null) return;

        int damage = Mathf.RoundToInt(attack * GetMultiplier(BlessingType.AttackUp));
        bool isCritical = Random.value < criticalRate;
        if (isCritical)
            damage = Mathf.RoundToInt(damage * criticalDamage);

        Debug.Log($"攻撃ダメージ: {damage} / クリティカル: {isCritical}");

        enemy.TakeDamage(damage, transform.position, isCritical);
    }

    public int GetAttackDamage()
    {
        bool isCritical = Random.value < criticalRate;

        int dmg = Mathf.RoundToInt(attack * GetMultiplier(BlessingType.AttackUp));
        if (isCritical)
            dmg = Mathf.RoundToInt(dmg * criticalDamage);

        return dmg;
    }

    //========================================
    // ■ リセット処理
    //========================================

    public void ResetStatus()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        isDead = false;
        isKnockedBack = false;

        if (rb != null)
            rb.simulated = true;

        if (sr != null)
            sr.color = Color.white;

        if (playerAnimation != null)
            playerAnimation.ResetAnimation();

        if (weaponEquip != null)
            weaponEquip.UnequipWeapon();

        PlayerInventory inventory = GetComponent<PlayerInventory>();
        if (inventory != null)
            inventory.ClearInventory();

        activeBlessings.Clear();
        RecalculateStats();
    }

    //========================================
    // ■ 武器ダメージ計算
    //========================================

    public int GetWeaponDamage(WeaponData weapon, out bool isCritical)
    {
        isCritical = Random.value < criticalRate;
        float total = weapon != null ? weapon.baseDamage + attack * weapon.attackScale + magic * weapon.magicScale : attack;
        

        if (isCritical)
            total *= criticalDamage;

        return Mathf.RoundToInt(total);
    }

    //========================================
    // ■ ポーション処理
    //========================================

    public bool UsePotion(int healAmount)
    {
        int before = currentHP;
        currentHP = Mathf.Min(maxHP, currentHP + healAmount);
        int healed = currentHP - before;

        if (healed > 0)
            healPopupSpawner.CreatePopup(healed, HealType.HP);

            AudioManager.Instance?.PlaySE(SEType.PotionUse);

        return healed > 0;
    }

    public bool UsePotionMP(int mpAmount)
    {
        int before = currentMP;
        currentMP = Mathf.Min(maxMP, currentMP + mpAmount);
        int healed = currentMP - before;

        if (healed > 0)
            healPopupSpawner.CreatePopup(healed, HealType.MP);

            AudioManager.Instance?.PlaySE(SEType.PotionUse);

        return healed > 0;
    }

    private void ApplyWeaponBonus(WeaponData weapon)
    {
        if (weapon == null) return;

        attack += weapon.bonusAttack;
        maxHP += weapon.bonusMaxHP;
        maxMP += weapon.bonusMaxMP;
        magic += weapon.bonusMagic;

        criticalRate += weapon.bonusCriticalRate;
        criticalDamage += weapon.bonusCriticalDamage;
    }

}

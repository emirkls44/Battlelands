using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Kalkan (Zýrh) Ayarlarý")]
    public float maxShield = 100f;
    public float currentShield = 0f;

    [Tooltip("Mermiler kalkana kaç kat daha fazla hasar versin? (Örn: 2 yaparsan 10 hasarlýk mermi kalkandan 20 götürür)")]
    public float shieldDamageMultiplier = 2f; // YENÝ: Kalkanýn kýrýlganlýk çarpaný

    [Header("HUD (Ekran) Ayarlarý")]
    public RectTransform healthBarRect;
    public TextMeshProUGUI healthText;

    public RectTransform shieldBarRect;
    public TextMeshProUGUI shieldText;

    [Header("Düþman Ölüm Ayarý")]
    public bool isEnemy = false;
    public GameObject lootBoxPrefab;

    private float maxHealthBarWidth;
    private float maxShieldBarWidth;

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = 0f;

        if (healthBarRect != null) maxHealthBarWidth = healthBarRect.sizeDelta.x;
        if (shieldBarRect != null) maxShieldBarWidth = shieldBarRect.sizeDelta.x;

        UpdateHealthUI();
    }

    // YENÝ: ÇARPANLI HASAR MANTIÐI
    public void TakeDamage(float baseDamage)
    {
        float remainingHealthDamage = baseDamage;

        // 1. Eðer mavi kalkanýmýz varsa, hasar önce kalkan çarpanýyla büyür
        if (currentShield > 0)
        {
            // Merminin kalkana vereceði asýl (katlanmýþ) hasarý hesapla
            float shieldDamage = remainingHealthDamage * shieldDamageMultiplier;

            if (shieldDamage <= currentShield)
            {
                // Kalkan tüm katlanmýþ hasarý göðüsledi
                currentShield -= shieldDamage;
                remainingHealthDamage = 0; // Cana geçecek hasar kalmadý
            }
            else
            {
                // Kalkan kýrýldý! Artan kalkan hasarýný bul
                float leftoverShieldDamage = shieldDamage - currentShield;
                currentShield = 0;

                // Artan kalkan hasarýný, normal can hasarýna geri çevir (bölerek)
                remainingHealthDamage = leftoverShieldDamage / shieldDamageMultiplier;
            }
        }

        // 2. Kalkan bittiyse (veya artan hasar varsa) normal hasarý yeþil cana uygula
        if (remainingHealthDamage > 0)
        {
            currentHealth -= remainingHealthDamage;
        }

        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();
        if (currentHealth <= 0) Die();
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void AddShield(float shieldAmount)
    {
        currentShield += shieldAmount;
        if (currentShield > maxShield) currentShield = maxShield;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (!isEnemy)
        {
            if (healthBarRect != null && maxHealth > 0)
            {
                float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);
                float newWidth = maxHealthBarWidth * healthPercent;
                healthBarRect.sizeDelta = new Vector2(newWidth, healthBarRect.sizeDelta.y);
            }

            if (healthText != null)
            {
                healthText.text = Mathf.RoundToInt(currentHealth) + " / " + maxHealth;
            }

            if (shieldBarRect != null && maxShield > 0)
            {
                float shieldPercent = Mathf.Clamp01(currentShield / maxShield);
                float newWidth = maxShieldBarWidth * shieldPercent;
                shieldBarRect.sizeDelta = new Vector2(newWidth, shieldBarRect.sizeDelta.y);

                shieldBarRect.parent.gameObject.SetActive(currentShield > 0);
            }

            if (shieldText != null)
            {
                shieldText.text = Mathf.RoundToInt(currentShield) + " / " + maxShield;
                shieldText.gameObject.SetActive(currentShield > 0);
            }
        }
    }

    void Die()
    {
        GameManager gm = Object.FindFirstObjectByType<GameManager>();

        if (isEnemy)
        {
            if (lootBoxPrefab != null) Instantiate(lootBoxPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            Destroy(gameObject);
            if (gm != null) gm.Invoke("CheckVictory", 0.1f);
        }
        else
        {
            if (gm != null) gm.GameOver();
            gameObject.SetActive(false);
        }
    }
}
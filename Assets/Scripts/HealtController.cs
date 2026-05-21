using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Düþman Ölüm Ayarý")]
    public bool isEnemy = false;          // Eðer bu kodu düþmana taktýysak Inspector'dan bunu true yapacaðýz
    public GameObject lootBoxPrefab;     // Düþman ölünce yere düþecek kutu (Loot)

    void Start()
    {
        // Oyun baþladýðýnda caný fulle
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " caný kalan: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        // Canýmýz maksimum caný (100) geçmesin
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log(gameObject.name + " iyileþti! Mevcut Can: " + currentHealth);
    }

    void Die()
    {
        // Eðer ölen kiþi bir düþmense ve bir ganimet kutusu belirlendiyse
        if (isEnemy && lootBoxPrefab != null)
        {
            // Tam öldüðü noktada yerde bir ganimet kutusu yarat
            Instantiate(lootBoxPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }

        // Objeyi sahneden sil
        Destroy(gameObject);
        Debug.Log(gameObject.name + " öldü!");
    }
}
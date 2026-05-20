using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        // Oyun baþladýðýnda caný fulle
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " hasar aldý! Kalan Can: " + currentHealth);

        // Can sýfýrýn altýna düþerse öl
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " elendi!");
        // Obseyi sahneden sil
        Destroy(gameObject);
    }
}
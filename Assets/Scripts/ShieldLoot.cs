using UnityEngine;

public class ShieldLoot : MonoBehaviour
{
    [Header("Kalkan Ayarý")]
    public float shieldAmount = 100f; // Alýnca ne kadar kalkan vereceði

    void OnTriggerEnter(Collider other)
    {
        // Eðer çarpan obje "Player" ise
        if (other.CompareTag("Player"))
        {
            // Oyuncunun can/kalkan kodunu bul
            HealthController playerHealth = other.GetComponent<HealthController>();

            if (playerHealth != null)
            {
                // Kalkaný doldur (100 yapar)
                playerHealth.AddShield(shieldAmount);

                Debug.Log("Mavi Kalkan Alýndý! Koruma Seviyesi: %100");

                // Yerdeki kalkan objesini haritadan sil (toplanmýþ oldu)
                Destroy(gameObject);
            }
        }
    }
}
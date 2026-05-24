using UnityEngine;

public class LootController : MonoBehaviour
{
    public enum LootType { Health, Ammo }

    [Header("Ganimet Ayarlarý")]
    public LootType type;
    public int amount = 20;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool isCollected = false;

            // 1. CAN KÝTÝ ALINDIYSA
            if (type == LootType.Health)
            {
                HealthController health = other.GetComponent<HealthController>();
                if (health != null)
                {
                    health.Heal(amount);
                    Debug.Log("Can kiti alýndý! +" + amount);
                    isCollected = true;
                }
            }
            // 2. MERMÝ KUTUSU ALINDIYSA (Kritik baðlantý düzeltildi)
            else if (type == LootType.Ammo)
            {
                PlayerShooting shooting = other.GetComponent<PlayerShooting>();
                if (shooting != null)
                {
                    shooting.AddAmmo(amount); // Mermiyi ekle ve arayüzü yenile
                    isCollected = true;
                }
                else
                {
                    // Eðer kod ana Player objesinde deðil de içindeki çocuk bir objedeyse üst babasýna bak
                    PlayerShooting parentShooting = other.GetComponentInParent<PlayerShooting>();
                    if (parentShooting != null)
                    {
                        parentShooting.AddAmmo(amount);
                        isCollected = true;
                    }
                }
            }

            if (isCollected)
            {
                Destroy(gameObject);
            }
        }
    }
}
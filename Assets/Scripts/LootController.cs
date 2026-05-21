using UnityEngine;

public class LootController : MonoBehaviour
{
    // Eþyanýn türünü Inspector'dan seçebilmek için bir liste oluþturuyoruz
    public enum LootType { Medkit, Weapon }
    public LootType type;

    public float healAmount = 50f; // Medkit ise ne kadar can verecek?

    // Birisi (oyuncu) bu eþyanýn içinden geçerse...
    void OnTriggerEnter(Collider other)
    {
        // Geçen kiþi Player mý?
        if (other.CompareTag("Player"))
        {
            if (type == LootType.Medkit)
            {
                // Saðlýk kitiyse canýný doldur
                HealthController health = other.GetComponent<HealthController>();
                if (health != null) health.Heal(healAmount);
            }
            else if (type == LootType.Weapon)
            {
                // Silahsa atýþ hýzýný artýr
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null) player.UpgradeWeapon();
            }

            // Etkiyi verdikten sonra eþyayý haritadan sil
            Destroy(gameObject);
        }
    }
}
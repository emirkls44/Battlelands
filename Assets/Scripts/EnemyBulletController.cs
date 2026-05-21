using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float damage = 10f; // Düþman mermisinin bize vereceði hasar
    private bool hasHit = false;

    void OnTriggerEnter(Collider other)
    {
        // Mermi baþka bir düþmana çarparsa hasar vermesin, pas geçsin
        if (hasHit || other.CompareTag("Enemy") || other.CompareTag("Bullet"))
            return;

        // Çarptýðý þey gerçekten Player (Biz) isek canýmýzý azaltsýn
        if (other.CompareTag("Player"))
        {
            HealthController playerHealth = other.GetComponent<HealthController>();
            if (playerHealth != null)
            {
                hasHit = true;
                playerHealth.TakeDamage(damage);
                Debug.Log("Oyuncu vuruldu! Canýn azaldý.");
            }
        }

        // Duvara veya yere çarpsa bile yok olsun
        Destroy(gameObject);
    }
}
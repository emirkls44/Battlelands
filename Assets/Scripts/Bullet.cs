using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f; // Merminin vereceði hasar

    void OnTriggerEnter(Collider other)
    {
        // 1. KONTROL: Eðer çarptýðýmýz þey "Player" ise hiçbir þey yapma, kodu durdur!
        if (other.CompareTag("Player"))
        {
            return;
        }

        // 2. HASAR ÝÞLEMÝ: Çarptýðýmýz objede HealthController var mý?
        HealthController health = other.GetComponent<HealthController>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // 3. YOK OLMA: Hedefe (veya duvara) çarptýktan sonra mermiyi sil
        Destroy(gameObject);
    }
}
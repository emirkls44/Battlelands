using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Mermi Ayarlarý")]
    public float damage = 25f; // Mermi baþýna verilecek hasar

    // Mermi bir þeyin içinden geçerse/çarparsa bu fonksiyon çalýþýr
    void OnTriggerEnter(Collider other)
    {
        // Eðer çarptýðýmýz þey çalýlýksa, onu görmezden gel ve içinden geç!
        if (other.gameObject.CompareTag("Bush")) return;
        // (Eðer OnTriggerEnter kullanýyorsan 'collision' yerine 'other' yaz).

        // Mermi kendi karakterimize (Player) çarpmasýn
        if (other.CompareTag("Player"))
            return;

        // Çarptýðýmýz objede "HealthController" kodu var mý? (Caný var mý?)
        HealthController targetHealth = other.GetComponent<HealthController>();

        if (targetHealth != null)
        {
            // Varsa ona hasar ver
            targetHealth.TakeDamage(damage);
            Debug.Log(other.name + " vuruldu! " + damage + " hasar aldý.");
        }

        // Mermi bir þeye çarptýktan sonra sahnede kalmasýn, kendini yok etsin
        Destroy(gameObject);
    }
}
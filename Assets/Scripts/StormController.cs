using UnityEngine;

public class StormController : MonoBehaviour
{
    [Header("Fýrtýna Ayarlarý")]
    public Transform player;
    public float shrinkSpeed = 1f;    // Daralma hýzý
    public float safeRadius = 25f;    // Baþlangýç çember yarýçapý
    public float minimumRadius = 2f;  // Fýrtýnanýn duracaðý en küçük boyut

    [Header("Hasar Ayarlarý")]
    public float stormDamage = 5f;    // Saniyede verilecek hasar
    private HealthController playerHealth;
    private float damageTimer = 0f;

    void Start()
    {
        // Oyun baþladýðýnda oyuncunun üzerindeki HealthController kodunu otomatik bul ve hafýzaya al
        if (player != null)
        {
            playerHealth = player.GetComponent<HealthController>();
        }
    }

    void Update()
    {
        // 1. Çemberi zamanla küçült
        if (safeRadius > minimumRadius)
        {
            safeRadius -= shrinkSpeed * Time.deltaTime;
            transform.localScale = new Vector3(safeRadius * 2, 15f, safeRadius * 2);
        }

        // 2. Oyuncu fýrtýna dýþýnda mý kontrol et
        if (player != null && playerHealth != null)
        {
            float distanceFromCenter = Vector3.Distance(
                new Vector3(player.position.x, 0, player.position.z),
                new Vector3(transform.position.x, 0, transform.position.z)
            );

            // Eðer uzaklýk güvenli yarýçaptan büyükse, oyuncu fýrtýnanýn içindedir (dýþarýdadýr)
            if (distanceFromCenter > safeRadius)
            {
                // Her 1 saniyede bir hasar ver (Sayaç Mantýðý)
                damageTimer += Time.deltaTime;
                if (damageTimer >= 1f)
                {
                    playerHealth.TakeDamage(stormDamage);
                    damageTimer = 0f; // Sayacý sýfýrla ki bir sonraki saniyeyi beklesin
                }
            }
            else
            {
                // Oyuncu güvenli alana geri dönerse sayacý sýfýrla
                damageTimer = 0f;
            }
        }
    }
}
using UnityEngine;

public class StormController : MonoBehaviour
{
    [Header("Fýrtýna Ayarlarý")]
    public float shrinkSpeed = 1.0f; // Alanýn saniyede ne kadar daralacaðý
    public float minSize = 10f;      // Alanýn küçülebileceði en son boyut
    public float stormDamage = 5f;   // Dýþarýda kalana saniyede verilecek hasar

    [Header("UI (Arayüz) Ayarlarý")]
    public GameObject warningText;   // Ekranda çýkacak "FIRTINADA KALDIN!" yazýsý

    private Transform player;
    private HealthController playerHealth;
    private float nextDamageTime = 0f;

    void Start()
    {
        // Oyuncuyu bul ve can sistemini hafýzaya al
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<HealthController>();
        }

        // Oyun baþlarken uyarý yazýsýný gizle (Sadece dýþarý çýkýnca görünsün)
        if (warningText != null)
        {
            warningText.SetActive(false);
        }
    }

    void Update()
    {
        // 1. ALANI YAVAÞ YAVAÞ DARALT
        if (transform.localScale.x > minSize)
        {
            transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;
        }

        // 2. OYUNCU ALANIN ÝÇÝNDE MÝ KONTROL ET
        if (player != null && playerHealth != null)
        {
            // Fýrtýnanýn merkezi ile oyuncu arasýndaki yatay mesafeyi ölç
            float distanceToPlayer = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(player.position.x, 0, player.position.z)
            );

            // Fýrtýnanýn o anki yarýçapý
            float currentRadius = transform.localScale.x / 2f;

            // Eðer oyuncu yarýçaptan daha uzaktaysa (Fýrtýnanýn dýþýnda kalmýþsa)
            if (distanceToPlayer > currentRadius)
            {
                // YAZIYI GÖSTER
                if (warningText != null) warningText.SetActive(true);

                // Saniyede 1 kere hasar ver
                if (Time.time >= nextDamageTime)
                {
                    playerHealth.TakeDamage(stormDamage);
                    Debug.Log("FIRTINADA KALDIN! " + stormDamage + " Hasar yedin.");
                    nextDamageTime = Time.time + 1f;
                }
            }
            else
            {
                // ÝÇERÝDEYSE YAZIYI GÝZLE
                if (warningText != null) warningText.SetActive(false);
            }
        }
    }
}
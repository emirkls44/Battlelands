using UnityEngine;

public class StormController : MonoBehaviour
{
    [Header("Fýrtýna Ayarlarý")]
    public Transform player;
    public float shrinkSpeed = 1f;    // Daralma hýzý
    public float safeRadius = 25f;    // Baþlangýç çember yarýçapý
    public float minimumRadius = 2f;  // Fýrtýnanýn duracaðý en küçük boyut

    void Update()
    {
        // 1. Çemberi zamanla küçült
        if (safeRadius > minimumRadius)
        {
            safeRadius -= shrinkSpeed * Time.deltaTime;
            // Silindirin boyutu (Scale) çapý temsil ettiði için yarýçapýn 2 katý olmalý
            transform.localScale = new Vector3(safeRadius * 2, 15f, safeRadius * 2);
        }

        // 2. Oyuncu fýrtýna (güvenli alan) dýþýnda mý kontrol et
        if (player != null)
        {
            // Oyuncunun merkeze olan uzaklýðýný hesapla (Y ekseni hariç)
            float distanceFromCenter = Vector3.Distance(
                new Vector3(player.position.x, 0, player.position.z),
                new Vector3(transform.position.x, 0, transform.position.z)
            );

            // Eðer uzaklýk güvenli yarýçaptan büyükse, oyuncu dýþarýdadýr
            if (distanceFromCenter > safeRadius)
            {
                Debug.Log("FIRTINA HASARI ALINIYOR! Can -1");
                // Ýleride buraya gerçek can (Health) düþme kodu eklenecek
            }
        }
    }
}
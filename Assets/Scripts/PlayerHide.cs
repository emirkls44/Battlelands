using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    [Header("Gizlilik Durumu")]
    public bool isHidden = false;

    [Header("Ayarlar")]
    public float revealDistance = 3f;
    public GameObject footprintIcon;

    private bool inBush = false;
    private Vector3 lastPosition;

    void Start()
    {
        if (footprintIcon != null) footprintIcon.SetActive(false);
        lastPosition = transform.position;
    }

    void Update()
    {
        if (inBush)
        {
            float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
            bool isMoving = speed > 0.1f;

            bool enemyTooClose = CheckEnemyProximity();

            if (enemyTooClose)
            {
                SetVisible(true);
                if (footprintIcon != null) footprintIcon.SetActive(false);
                isHidden = false;
            }
            else
            {
                isHidden = true;
                SetVisible(false); // Karakteri saydam yap

                if (footprintIcon != null) footprintIcon.SetActive(isMoving);
            }
        }
        else
        {
            isHidden = false;
            SetVisible(true);
            if (footprintIcon != null) footprintIcon.SetActive(false);
        }

        lastPosition = transform.position;
    }

    bool CheckEnemyProximity()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= revealDistance)
            {
                return true;
            }
        }
        return false;
    }

    // ÝÞTE YENÝLENEN KISIM: Karakterdeki TÜM parçalarý bulup saydamlaþtýrýr!
    void SetVisible(bool isVisible)
    {
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in allRenderers)
        {
            // Eðer o anki parça footprint (ayak izi) deðilse rengini deðiþtir
            if (footprintIcon != null && r.gameObject == footprintIcon) continue;

            Color c = r.material.color;
            c.a = isVisible ? 1f : 0.3f;
            r.material.color = c;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bush"))
        {
            inBush = true;
            Debug.Log("Çalýlýða GÝRDÝ!"); // Konsolda çalýþtýðýný görmek için
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bush"))
        {
            inBush = false;
            Debug.Log("Çalýlýktan ÇIKTI!");
        }
    }
}
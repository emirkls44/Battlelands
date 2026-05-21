using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Düþmanýn yapabileceði durumlar
    public enum EnemyState { Patrolling, Chasing }
    private EnemyState currentState = EnemyState.Patrolling;

    private NavMeshAgent agent;
    private Transform target;
    private HealthController playerHealth;

    [Header("Animasyon")]
    public Animator anim;

    [Header("Görüþ ve Devriye Ayarlarý")]
    public float detectionRadius = 12f; // Seni fark etme alaný (Sarý Çember)
    public float patrolRadius = 10f;    // Kendi kendine takýlacaðý alanýn geniþliði
    public float patrolWaitTime = 4f;   // Bir noktaya gidince ne kadar bekleyip yenisine geçeceði
    private float patrolTimer;

    [Header("Ateþ Etme Ayarlarý")]
    public GameObject enemyBulletPrefab; // Yeni yaptýðýmýz EnemyBullet prefab'ý
    public Transform firePoint;          // Silahýnýn ucu
    public float shootDistance = 8f;     // Ateþ etme menzili (Kýrmýzý Çember)
    public float fireRate = 0.8f;        // Ateþ etme sýklýðý
    public float bulletSpeed = 15f;

    [Range(0f, 0.4f)]
    public float inaccuracySpread = 0.2f; // ISKALAMA AYARI! (Sayý büyürse daha çok ýskalar)
    private float nextFireTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolTimer = patrolWaitTime;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            playerHealth = playerObj.GetComponent<HealthController>();
        }

        // Oyuna baþlarken rastgele bir yere git emri ver
        SetRandomPatrolDestination();
    }

    void Update()
    {
        if (target == null || agent == null) return;

        // Oyuncuyla aradaki mesafeyi ölç
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        // DURUM KONTROLÜ: Alana girdi mi?
        if (distanceToPlayer <= detectionRadius)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            currentState = EnemyState.Patrolling;
        }

        // DURUMA GÖRE DAVRANMA
        if (currentState == EnemyState.Chasing)
        {
            ChaseAndShootLogic(distanceToPlayer);
        }
        else if (currentState == EnemyState.Patrolling)
        {
            PatrolLogic();
        }

        // Animasyon Hýz Ayarý
        if (anim != null)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    // --- KENDÝ KENDÝNE TAKILMA (LOOT/DEVRIYE) MANTIÐI ---
    void PatrolLogic()
    {
        agent.isStopped = false;
        patrolTimer += Time.deltaTime;

        // Bekleme süresi dolduysa yeni bir rastgele noktaya yürü
        if (patrolTimer >= patrolWaitTime)
        {
            SetRandomPatrolDestination();
            patrolTimer = 0f;
        }
    }

    void SetRandomPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        // NavMesh üzerinde yürünebilir geçerli bir nokta bul
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    // --- KOVALAMA VE ATEÞ ETME MANTIÐI ---
    void ChaseAndShootLogic(float distanceToPlayer)
    {
        // Eðer ateþ etme mesafesindeysek dur, oyuncuya dön ve ateþ et
        if (distanceToPlayer <= shootDistance)
        {
            // 1. TAMAMEN DUR VE KAYMAYI KES
            agent.isStopped = true;
            agent.velocity = Vector3.zero; // Momentum sýfýrlanýr, kayma/çember çizme engellenir!

            // 2. NAVMESH'IN DÖNÜÞ KONTROLÜNÜ KAPAT (Kavgayý bitiren kod)
            agent.updateRotation = false;

            // 3. YÜZÜNÜ OYUNCUYA DÖN (Artýk sadece bu kod çalýþacak)
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }

            // 4. ATEÞ ET
            if (Time.time >= nextFireTime)
            {
                ShootWithSpread();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            // KOÞMAYA DEVAM ET (NavMesh'in kontrolünü geri ver)
            agent.isStopped = false;
            agent.updateRotation = true; // Zeka tekrar aktif!
            agent.SetDestination(target.position);
        }
    }

    void ShootWithSpread()
    {
        if (anim != null) anim.SetTrigger("Shoot");

        if (firePoint == null || enemyBulletPrefab == null) return;

        // Mermiyi yarat
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // ISKALAMA MANTIÐI: Düz giden oka rastgele saða/sola sapma (Spread) ekliyoruz
        Vector3 baseDirection = firePoint.forward;
        Vector3 randomSpread = new Vector3(
            Random.Range(-inaccuracySpread, inaccuracySpread),
            0f,
            Random.Range(-inaccuracySpread, inaccuracySpread)
        );
        Vector3 finalDirection = (baseDirection + randomSpread).normalized;

        // Mermiyi fýrlat (Unity 6 Hýz sistemi)
        bulletRb.linearVelocity = finalDirection * bulletSpeed;
        Destroy(bullet, 2f);
    }

    // Editor ekranýnda alanlarý renkli çember olarak görmek için (Kolaylýk saðlar)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // Görüþ alaný sarý
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;    // Ateþ alaný kýrmýzý
        Gizmos.DrawWireSphere(transform.position, shootDistance);
    }
}
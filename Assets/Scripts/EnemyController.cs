using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState { Patrolling, Chasing }
    private EnemyState currentState = EnemyState.Patrolling;

    private NavMeshAgent agent;
    private Transform target;
    private HealthController playerHealth;

    // YENÝ EKLENEN: Oyuncunun gizlilik kodunu okumak için
    private PlayerHide playerHideScript;

    [Header("Animasyon")]
    public Animator anim;

    [Header("Görüþ ve Devriye Ayarlarý")]
    public float detectionRadius = 12f;
    public float patrolRadius = 10f;
    public float patrolWaitTime = 4f;
    private float patrolTimer;

    [Header("Ateþ Etme Ayarlarý")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float shootDistance = 8f;
    public float fireRate = 0.8f;
    public float bulletSpeed = 15f;

    [Range(0f, 0.4f)]
    public float inaccuracySpread = 0.2f;
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

            // Oyuncunun gizlilik kodunu bul ve hafýzaya al
            playerHideScript = playerObj.GetComponent<PlayerHide>();
        }

        SetRandomPatrolDestination();
    }

    void Update()
    {
        if (target == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        // GÝZLÝLÝK KONTROLÜ (YENÝ SÝSTEM)
        // Eðer oyuncu görüþ alanýndaysa (detectionRadius) VE gizli DEÐÝLSE kovala!
        bool isPlayerHidden = (playerHideScript != null && playerHideScript.isHidden);

        if (distanceToPlayer <= detectionRadius && !isPlayerHidden)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            // Oyuncu uzaktaysa VEYA gizliyse (isHidden == true) devriyeye dön, onu boþver
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

        if (anim != null)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    void PatrolLogic()
    {
        // Devriyeye döndüðünde dönüþ kilidini aç
        agent.updateRotation = true;
        agent.isStopped = false;

        patrolTimer += Time.deltaTime;

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
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }

    void ChaseAndShootLogic(float distanceToPlayer)
    {
        if (distanceToPlayer <= shootDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.updateRotation = false;

            Vector3 direction = (target.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }

            if (Time.time >= nextFireTime)
            {
                ShootWithSpread();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.updateRotation = true;
            agent.SetDestination(target.position);
        }
    }

    void ShootWithSpread()
    {
        if (anim != null) anim.SetTrigger("Shoot");

        if (firePoint == null || enemyBulletPrefab == null) return;

        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        Vector3 baseDirection = firePoint.forward;
        Vector3 randomSpread = new Vector3(
            Random.Range(-inaccuracySpread, inaccuracySpread),
            0f,
            Random.Range(-inaccuracySpread, inaccuracySpread)
        );
        Vector3 finalDirection = (baseDirection + randomSpread).normalized;

        bulletRb.linearVelocity = finalDirection * bulletSpeed;
        Destroy(bullet, 2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootDistance);
    }
}
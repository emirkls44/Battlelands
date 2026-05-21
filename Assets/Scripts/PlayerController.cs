using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket ve UI Ayarlarý")]
    public float moveSpeed = 5f;
    public SimpleJoystick leftJoystick;
    public SimpleJoystick rightJoystick;

    [Header("Animasyon")]
    public Animator anim; // ÝÞTE EKSÝK OLAN KISIM BURASI

    [Header("Silah Ayarlarý")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. MOBÝL HAREKET GÝRDÝSÝ
        moveInput = new Vector3(leftJoystick.inputVector.x, 0f, leftJoystick.inputVector.y).normalized;

        // 2. MOBÝL NÝÞAN VE ATEÞ ETME
        Vector3 aimInput = new Vector3(rightJoystick.inputVector.x, 0f, rightJoystick.inputVector.y);

        if (aimInput.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(aimInput);

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        else if (moveInput.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(moveInput);
        }

        // 3. ANÝMASYONA HIZ VERÝSÝNÝ GÖNDER
        if (anim != null)
        {
            anim.SetFloat("Speed", moveInput.magnitude);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.linearVelocity = firePoint.forward * bulletSpeed;
        Destroy(bullet, 2f);
    }

    public void UpgradeWeapon()
    {
        fireRate = 0.15f;
        bulletSpeed = 35f;
        Debug.Log("Silah Yükseltildi! Seri Ateþ Modu Aktif.");
    }
}
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket ve UI Ayarlarý")]
    public float moveSpeed = 5f;
    public SimpleJoystick leftJoystick;  // Sol Joystick referansý
    public SimpleJoystick rightJoystick; // Sað Joystick referansý

    [Header("Silah Ayarlarý")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f; // Saniyede kaç mermi atacak
    private float nextFireTime = 0f;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. MOBÝL HAREKET GÝRDÝSÝ (Sol Joystick'ten oku)
        moveInput = new Vector3(leftJoystick.inputVector.x, 0f, leftJoystick.inputVector.y).normalized;

        // 2. MOBÝL NÝÞAN VE ATEÞ ETME (Sað Joystick'ten oku)
        Vector3 aimInput = new Vector3(rightJoystick.inputVector.x, 0f, rightJoystick.inputVector.y);

        // Eðer sað joystick çekiliyorsa (parmak ekrandaysa)
        if (aimInput.magnitude > 0.1f)
        {
            // Karakteri niþan alýnan yöne çevir
            transform.rotation = Quaternion.LookRotation(aimInput);

            // Ateþ etme süresi geldiyse otomatik ateþ et
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        else if (moveInput.magnitude > 0.1f)
        {
            // Eðer ateþ edilmiyor ama yürünüyorsa, karakter yürüdüðü yöne baksýn
            transform.rotation = Quaternion.LookRotation(moveInput);
        }
    }

    void FixedUpdate()
    {
        // Fiziksel Hareketi Uygula
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Unity 6 linearVelocity
        bulletRb.linearVelocity = firePoint.forward * bulletSpeed;

        Destroy(bullet, 2f);
    }
}
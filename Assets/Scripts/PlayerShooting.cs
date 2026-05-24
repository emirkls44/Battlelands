using UnityEngine;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    [Header("Mermi Ayarlarý")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public TextMeshProUGUI ammoText;

    [Header("Mobil Kontrol")]
    public SimpleJoystick rightJoystick;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        // MERMÝ 0 VEYA DAHA AZSA ASLA AÞAÐIDAKÝ ATEÞ ETME LOGIC'LERÝNE GEÇME (ÇELÝK DUVAR 1)
        if (currentAmmo <= 0) return;

        // SAÐ JOYSTICK KONTROLÜ
        if (rightJoystick != null)
        {
            if ((Mathf.Abs(rightJoystick.inputVector.x) > 0.1f || Mathf.Abs(rightJoystick.inputVector.y) > 0.1f)
                && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
        // MOUSE KONTROLÜ
        else
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        // ATEÞ ETME ANINDAKÝ SON KONTROL (ÇELÝK DUVAR 2)
        if (currentAmmo <= 0) return;

        currentAmmo--;
        UpdateAmmoUI();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
        Destroy(bullet, 2f);
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            if (currentAmmo <= 0)
            {
                currentAmmo = 0; // Merminin eksiye düþmesini engelle
                ammoText.text = "<color=red>0</color> / " + maxAmmo;
            }
            else
            {
                ammoText.text = currentAmmo + " / " + maxAmmo;
            }
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }
}
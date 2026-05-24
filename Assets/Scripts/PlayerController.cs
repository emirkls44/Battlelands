using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 5f;

    [Header("Mobil Kontroller")]
    public SimpleJoystick leftJoystick;
    public SimpleJoystick rightJoystick;

    [Header("Animasyon")]
    public Animator anim;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. HAREKET GÝRDÝSÝ (HEM JOYSTICK HEM KLAVYE)
        Vector3 joystickMove = Vector3.zero;
        if (leftJoystick != null)
        {
            joystickMove = new Vector3(leftJoystick.inputVector.x, 0f, leftJoystick.inputVector.y);
        }

        // Klavyeden WASD veya Yön Tuþlarýný okur
        Vector3 keyboardMove = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Ýkisinden gelen gücü birleþtir ve normalize et (çapraz basýnca 2 kat hýzlanmayý önler)
        moveInput = (joystickMove + keyboardMove).normalized;


        // 2. NÝÞAN ALMA VE DÖNÜÞ (Sað Joystick)
        Vector3 aimInput = Vector3.zero;
        if (rightJoystick != null)
        {
            aimInput = new Vector3(rightJoystick.inputVector.x, 0f, rightJoystick.inputVector.y);
        }

        // Eðer sað joystick çekiliyorsa karakteri oraya döndür
        if (aimInput.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(aimInput);
        }
        // Eðer ateþ edilmiyorsa ama yürünüyorsa (WASD veya Sol Joystick ile), karakteri gittiði yöne döndür
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
        // Karakteri Fiziksel Olarak Yürüt
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
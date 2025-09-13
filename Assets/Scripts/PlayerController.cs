using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform laserPointer;
    public LineRenderer laserLine;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    public bool isLocked = false;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookDir;
    
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        // 🔹 Mostrar vida inicial en UI
        UIManager.Instance.UpdateLife(currentHealth);
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        isLocked = Input.GetButton("Fire3");

        float lookX = Input.GetAxis("RightStickHorizontal");
        float lookY = Input.GetAxis("RightStickVertical");

        if (lookX != 0 || lookY != 0)
            lookDir = new Vector2(lookX, lookY).normalized;
        else if (moveInput != Vector2.zero && !isLocked)
            lookDir = moveInput;

        if (lookDir != Vector2.zero)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90f;
        }

        if (laserLine != null && laserPointer != null)
        {
            laserLine.SetPosition(0, laserPointer.position);
            laserLine.SetPosition(1, laserPointer.position + laserPointer.up * 10f);
        }

        if (Input.GetButtonDown("Fire1") && bulletPrefab != null && firePoint != null)
            Shoot();
    }

    void FixedUpdate()
    {
        if (!isLocked)
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        Vector2 direction = firePoint.up;
        BulletPool.Instance.SpawnBullet(firePoint.position, firePoint.rotation, direction, bulletSpeed);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player HP: " + currentHealth);
        
        UIManager.Instance.UpdateLife(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("¡Jugador muerto!");
        UIManager.Instance.ShowDefeat();
    }

    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        transform.position = Vector3.zero;
        
        UIManager.Instance.UpdateLife(currentHealth);
    }
}


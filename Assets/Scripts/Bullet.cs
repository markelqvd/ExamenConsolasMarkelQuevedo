using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 3f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction, float bulletSpeed)
    {
        gameObject.SetActive(true);
        rb.linearVelocity = direction.normalized * bulletSpeed;
        CancelInvoke();
        Invoke(nameof(Disable), lifetime);
    }

    private void Disable()
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>()?.TakeDamage(damage);
            Disable();
        }
    }

    private void OnBecameInvisible()
    {
        Disable();
    }
}
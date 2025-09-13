using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Derrota");
        }
    }
}

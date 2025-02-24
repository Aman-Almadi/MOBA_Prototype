using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float hurtDuration = 0.2f;
    private int currentHealth;
    public Slider healthBar;
    public GameObject gameOverScreen;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;  

        UpdateHealthBar();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        animator.SetTrigger("Hurt");
        UpdateHealthBar();
        StartCoroutine(HurtEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        animator.SetTrigger("Die");
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateHealthBar()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        animator = GetComponent<Animator>();
    }

    IEnumerator HurtEffect()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(hurtDuration);
        
        spriteRenderer.color = originalColor;
    }
}

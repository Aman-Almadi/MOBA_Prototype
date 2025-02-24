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
    public AudioClip hurtSound;
    public AudioClip deathSound;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private AudioSource audioSource;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;  

        UpdateHealthBar();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        animator.SetTrigger("Hurt");
        UpdateHealthBar();
        audioSource.PlayOneShot(hurtSound);
        StartCoroutine(HurtEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        audioSource.PlayOneShot(deathSound);
        animator.SetTrigger("Die");
        Invoke("GameOver", 1.5f);
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

    private void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
}

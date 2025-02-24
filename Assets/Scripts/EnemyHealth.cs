using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public Slider healthBar;
    public AudioClip hitSound;
    private int currentHealth;
    private Animator animator;
    private bool isDead = false;
    private AudioSource audioSource;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        //Assign health bar (Setting it manually in Unity)
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        audioSource.PlayOneShot(hitSound);
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        animator.SetTrigger("Die");

        GetComponent<Collider>().enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        Destroy(gameObject, 3f);    //Destroy after 3 seconds
    }
}

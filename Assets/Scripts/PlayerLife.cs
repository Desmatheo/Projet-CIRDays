using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] Animator animator;

    public int maxHealth = 500;
    private float currentHealth = 500;

    public bool isDead = false;
    public bool test = false;

    public float valeurDegats;


    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float attackDamage, float animationSpeed, float distance)
    {
        currentHealth -= attackDamage;
        
        healthBar.SetHealth(currentHealth);

        animator.SetTrigger("Hurt");
        animator.speed = animationSpeed;
        if(this.GetComponentInParent<PlayerCombat>().typeCoup == 0)
        {
            valeurDegats = 0.8f;
        }else if(this.GetComponentInParent<PlayerCombat>().typeCoup == 1)
        {
            valeurDegats = 1.3f;
        }
        else if(this.GetComponentInParent<PlayerCombat>().typeCoup == 2)
        {
            valeurDegats = 2f;
        }
        if(distance > 0)
        {
            this.transform.position = new Vector2(this.transform.position.x + valeurDegats, this.transform.position.y);
        }
        else
        {
            this.transform.position = new Vector2(this.transform.position.x - valeurDegats, this.transform.position.y);
        }

        if(currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        isDead = true;
    }

    public float getHealth(){
        return currentHealth;
    }
}

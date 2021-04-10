using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKing : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 300;
    int currentHealth;
    private Collider2D collide;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealth <= 0 )
        {
            Die();
        }
    }

    void Die()
    {
        
        animator.SetBool("Death",true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        
    }
   
}

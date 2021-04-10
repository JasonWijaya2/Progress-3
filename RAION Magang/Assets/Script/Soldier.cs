using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public float speed;
    private bool MovingLeft = true;
    public float distance;
    public Transform groundDetection;
    public Animator animator;
    public int maxHealth = 60;
    int currentHealth;
    private Collider2D collider;
    void Start()
    {
        currentHealth = maxHealth;
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if(MovingLeft == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                MovingLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                MovingLeft = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("Death", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

    }

}

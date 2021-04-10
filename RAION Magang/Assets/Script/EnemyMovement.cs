using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform player; //Targetkan ke player
    [SerializeField] float agroRange; //Range mendeteksi player saat player mendekat
    [SerializeField] float Movespeed;

    public int maxHealth = 60;
    int currentHealth;
    private Collider2D coll;
    private Animator animator;

    public Transform enemyattackPos;
    public float attackRange;
    public LayerMask playerLayer;
    public int attackdamage = 20;
    public float attackrate = 2f;
    float nextTimeAttack = 0f;

    Rigidbody2D rb2d;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position); //Jarak antara player dengan enemy

        if(distanceToPlayer < agroRange) //Jika jarak lebih kecil dari Range
        {
            ChasePlayer(); //Memanggil fungsi ChasePlayer
        }

        else
        {
            StopChasingPlayer();
        }

        if (Time.time >= nextTimeAttack) //Jika saat attack berlangsung
        {
            if (distanceToPlayer >= attackRange)
            {
                Attack(); //Memanggil fungsi attack
                nextTimeAttack = Time.time + 1f / attackrate; //Attack yang berikutnya akan berlangsung setengah detik setelah attack berlangsung
            }
        }
    }

    void StopChasingPlayer() //Fungsi untuk berhenti menangkap player
    {
        rb2d.velocity = new Vector2(0,0);
    }

    void ChasePlayer() //Fungsi untuk menangkap player
    {
        if(transform.position.x < player.position.x)//Enemy berada di kiri dan player berada di kanan
        {
            rb2d.velocity = new Vector2(Movespeed, 0); //Enemy bergerak ke kanan
            transform.localScale = new Vector2(4, 4);
        }

        else //Enemy berada di kanan dan player berada di kiri
        {
            
            rb2d.velocity = new Vector2(-Movespeed, 0);//Enemy bergerak ke kiri
            transform.localScale = new Vector2(-4, 4);
        }

        animator.SetBool("Walk", true); 
    }

    public void TakeDamage(int damage) //Saat enemy terkena damage
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Attack() //Enemy attak
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitplayer = Physics2D.OverlapCircleAll(enemyattackPos.position, attackRange, playerLayer);
        foreach (Collider2D player in hitplayer)
        {
            player.GetComponent<PlayerMovement>().TakeDamage(20);
        }

    }

    private void OnDrawGizmosSelected() //Fungsi untuk display overlapcircle dan attack point
    {
        if (enemyattackPos == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(enemyattackPos.position, attackRange);
    }

    void Die()
    {
        animator.SetBool("Death", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        rb2d.velocity = new Vector2(0, 0);

        GetComponent<EnemyMovement>().enabled = false;

    }
}

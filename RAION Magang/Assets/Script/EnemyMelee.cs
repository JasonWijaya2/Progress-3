using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public Transform enemyattackPos;
    public float attackRange;
    public LayerMask playerLayer;
    public int attackdamage = 20;
    public float attackrate = 2f;
    float nextTimeAttack = 0f;
    

    
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (Time.time >= nextTimeAttack) //Jika saat attack berlangsung
        {
            if (distanceToPlayer >= attackRange)
            {
                Attack(); //Memanggil fungsi attack
                nextTimeAttack = Time.time + 1f / attackrate; //Attack yang berikutnya akan berlangsung setengah detik setelah attack berlangsung
            }
        }
    }

    void Attack()
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
}

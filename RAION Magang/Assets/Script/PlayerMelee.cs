using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public Animator animator;
    bool comboPosible; //combo belum sempat dibikin :"> , jadi masih menggunakan satu basic attack
    int comboStep;

    public Transform attack; //Posisi attack
    public float attackrange; //Jarak attack
    public LayerMask enemylayer; //Layer Enemy
    public int attackdamage = 20; //Damage attack
    public float attackrate = 2f; //Kecepatan attack
    float nextAttacktime = 0f; //Waktu attack yang berikutnya

    void Update()
    {
        if(Time.time >= nextAttacktime) //Jika saat attack berlangsung
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Attack(); //Memanggil fungsi attack
                nextAttacktime = Time.time + 1f / attackrate; //Attack yang berikutnya akan berlangsung setengah detik setelah attack berlangsung
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); //Memainkan animasi attack
        //Overlapcircle untuk membuat circle untuk hit musuh
        Collider2D[] hitenemy = Physics2D.OverlapCircleAll(attack.position, attackrange, enemylayer); //Deteksi posisi attack dengan jarak enemy
        foreach(Collider2D enemy in hitenemy) //Damage untuk musuh
        {
            enemy.GetComponent<EnemyMovement>().TakeDamage(20);
        }
        
    }

    private void OnDrawGizmosSelected() //Fungsi untuk display overlapcircle dan attack point
    {
        if(attack == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attack.position, attackrange);
    }
}

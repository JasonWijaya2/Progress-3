using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private enum State { Idle,Run,Jump,Fall} //Menggunakan state untuk animasi agar tidak terlalu banyak parameters
    private State state = State.Idle; //State atau animasi awal adalah idle
    private Collider2D collider;
    [SerializeField] private LayerMask Ground;
    public float speed;
    public float jump;

    public int maxHealth = 1000; //Healt maximum untuk player
    int currentHealth; // Health player

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>(); //Memanggil Component RigidBody2d
        anim = GetComponent<Animator>(); //Memanggil animator
        collider = GetComponent<Collider2D>(); //Memanggil Collider2d
    }


    void IgnoreCollision()
    {
        Physics2D.IgnoreLayerCollision(9, 10); //Mencegah enemy mental saat menyentuh player
    }

    private void Update()
    {
        float Direction = Input.GetAxis("Horizontal"); //Bergerak secara horizontal menggunakan "A" dan "D"

        if(Direction < 0) //Jika ke kiri
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y); //Rigidbody akan menggerakan player ke kiri
            transform.localScale = new Vector2(-3,3); //Player menghadap ke kiri
        }

        else if (Direction > 0) //Jika ke kanan
        {
            rb.velocity = new Vector2(speed, rb.velocity.y); //Rigidbody akan menggerakan player ke kanan
            transform.localScale = new Vector2(3, 3); //Player menghadap ke kanan
        }

        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); //Jika tidak bergerak, maka player tetap diam dengan sumbu x=0
        }
        
        if (Input.GetKey(KeyCode.W) && collider.IsTouchingLayers(Ground)) //Input "W" untuk melompat dan saat player menginjak ground
        {
            rb.velocity = new Vector2(rb.velocity.x, jump); //Maka player akan melompat dengan float jump yang dapat diatur
            state = State.Jump; //Animasi Jump menggunakan state
        }
        
        VelocityState(); //Memanggil fungsi state
        anim.SetInteger("state", (int) state); //Mengaktifkan animasi state yang ada di parameter
        
    }

    private void VelocityState() //Fungsi state untuk mengaktifkan animasi
    {
        if (state == State.Jump) //Jika player lompat
        {
            if (rb.velocity.y < 1f) //Dan jika player yang sedang melompat di udara sedang ingin turun
            {
                state = State.Fall; //Maka mengaktifkan animasi fall
            }
        }
        else if (state == State.Fall) //Jika player sedang fall
        {
            if(collider.IsTouchingLayers(Ground)) //Dan player menyentuh ground
            {
                state = State.Idle; //Maka akan kembali ke animasi idle
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 1f) //Jika player bergerak secara horizontal
        {
            state = State.Run; //Maka akan mengaktifkan animasi Run
        }
        else //Sisanya
        {
            state = State.Idle; //Idle
        }
    }

    public void TakeDamage(int damage) //Fungsi saat player terkena damage dari enemy
    {
        currentHealth -= damage; //Current Health akan terus berkurang saat terkena damage

        anim.SetTrigger("Hurt"); //animasi hurt

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die() //Fungsi saat player mati
    {

        anim.SetBool("Death", true);

        GetComponent<Collider2D>().enabled = false; //Saat player mati, maka enemy tidak bisa collide player
        this.enabled = false; 
        rb.velocity = new Vector2(0, 0); //Rigidbody akan diam

        GetComponent<PlayerMovement>().enabled = false; //Saat player mati, player tidak bisa bergerak lagi

        SceneManager.LoadScene("GameOver");

    }
}

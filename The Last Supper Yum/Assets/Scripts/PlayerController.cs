using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;       
    public LayerMask groundLayer;      

    private Rigidbody2D rb;           
    private Vector3 initialScale;      
    private bool isGrounded;           

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();


        initialScale = transform.localScale;
    }

    void Update()
    {
    
        float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(move, 0, 0);

     
        CheckGrounded();
    }

    void CheckGrounded()
    {
      
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        if (isGrounded)
        {
        
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
        }
        else
        {
            // Handle logic if the player is not grounded
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Enemy"))
        {
         
            GrowPlayer();

           
            Destroy(collision.gameObject);
        }
    }

    void GrowPlayer()
    {
       
        transform.localScale += new Vector3(0.2f, 0.2f, 0); 
    }
}

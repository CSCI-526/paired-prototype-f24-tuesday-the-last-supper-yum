using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;            

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 100f;      
    public float fallThreshold = -10f;  
    public GameObject restartButton;   
    public GameObject inGameRestartButton;
    // public LayerMask groundLayer;
    public Animator playerAnims;

    private Rigidbody2D rb;           
    private Vector3 initialScale;     
    public bool isGrounded = true; 
    // public bool hasShrunk = false; // flag to prevent repeated shrinking

    public string platform = "Gound";
    public string newPlat = "Ground";
    public bool highPt = false;
    public float hight = 0f;
    public float baseHight = 0f;

    public bool inverse = false;
    public float size = 1.2f;
    public string resizeDirection = "";
    public float speedMult = 5f;
    private float stretchedAmount = 0f;

    enum MoveMode
    {
        idle,
        shrink,
        stretch
    }

    private MoveMode moveMode = MoveMode.idle;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        initialScale = transform.localScale;

        restartButton.SetActive(false);
    }

    /*void FixedUpdate()
    {
       
        float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(move, 0, 0);

        if (transform.position.y < fallThreshold)
        {
            PlayerDies();
        }
    }*/

    void FixedUpdate()
    {
        float resizeAmount = moveSpeed * Time.deltaTime;

        // horizontal movement
        if (Input.GetAxis("Horizontal") == 1)
        {
            if(moveMode == MoveMode.idle)
            {
                resizeDirection = "x";
                inverse = false;
                moveMode = MoveMode.stretch;
            }
        }
        else if (Input.GetAxis("Horizontal") == -1)
        {
            if (moveMode == MoveMode.idle)
            {
                resizeDirection = "x";
                inverse = true;
                moveMode = MoveMode.stretch;
            }
        }

        // vertical movement
        else if (Input.GetAxis("Vertical") == 1) 
        {
            if (moveMode == MoveMode.idle && isGrounded)
            {
                resizeDirection = "y";
                inverse = false;
                moveMode = MoveMode.stretch;
            }
        }
        else if (Input.GetAxis("Vertical") == -1)
        {
            if(!isGrounded){
                if (moveMode == MoveMode.idle)
                {
                    resizeDirection = "y";
                    inverse = true;
                    moveMode = MoveMode.stretch;
                }
            }
            
        }

        // idle
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            //resizeAmount = 0;
            //resizeDirection = "";
            moveMode = MoveMode.shrink;
        }

        if (moveMode == MoveMode.stretch)
        {
            Stretch(resizeAmount, resizeDirection);
        }
        else if (moveMode == MoveMode.shrink)
        {
            Shrink(resizeAmount, resizeDirection);
        }

        // player dies
        if (transform.position.y < fallThreshold)
        {
            PlayerDies();
        }

        if(rb.velocity.y < 0 && !highPt){
            highPt = true;
            hight = transform.position.y;
            Debug.Log("starting height: " + baseHight);
            Debug.Log("highest height: " + hight);
        }
    }

    // Source: https://pastebin.com/4VsCvrs7 
    void Stretch(float amount, string direction)
    {
        if (stretchedAmount < size)
        {
            if (direction == "x" && inverse == false)
            {
                transform.position = new Vector2(transform.position.x + (amount / 2), transform.position.y);
                transform.localScale = new Vector2(transform.localScale.x + amount, transform.localScale.y);
            }

            if (direction == "y" && inverse == false)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (amount / 2));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + amount);
            }

            if (direction == "x" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x - (amount / 2), transform.position.y);
                transform.localScale = new Vector2(transform.localScale.x + amount, transform.localScale.y);
            }

            if (direction == "y" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - (amount / 2));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + amount);
            }

            stretchedAmount += amount;
        }
        else // stretch limit
        {
            moveMode = MoveMode.shrink;
        }
    }

    void Shrink(float amount, string direction)
    {
        if (stretchedAmount > 0f)
        {
            rb.gravityScale = 0;
            if (direction == "x" && inverse == false)
            {
                transform.position = new Vector2(transform.position.x + (amount / 2), transform.position.y);
                transform.localScale = new Vector2(transform.localScale.x - amount, transform.localScale.y);
            }
            if (direction == "y" && inverse == false)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (amount / 2));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - amount);
            }

            if (direction == "x" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x - (amount / 2), transform.position.y);
                transform.localScale = new Vector2(transform.localScale.x - amount, transform.localScale.y);
            }
            if (direction == "y" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - (amount / 2));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - amount);
            }
            stretchedAmount -= amount;
        }
        else // no more shrinking
        {
            rb.gravityScale = 1;
            moveMode = MoveMode.idle;
        }
    }

    public void PlayerDies()
    {
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        playerAnims.Play("Dead");
    }

    public void PlayerDestroyed()
    {
        restartButton.SetActive(true);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Trigger Entered");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float enemySize = collision.gameObject.GetComponent<EnemyMovement>().size;
            if (size > enemySize)
            {
                Debug.Log(" Collision " + collision.gameObject.name);
                GrowPlayer(enemySize / size);
                size += enemySize;
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("Die ");
                PlayerDies();
            }
        } 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            baseHight = transform.position.y;
            if(hight - baseHight > 1){
                ShrinkPlayer(0.1f);
            }
        }

        // // Debug.Log("Before collision the player is on " + platform);
        // newPlat = collision.collider.gameObject.tag;
        // // Debug.Log("new platform collided with " + newPlat);

        // if (newPlat != platform && !isGrounded) 
        // {
        //     // Shrink only if the platform is new and hasn't shrunk yet
        //     Debug.Log("Player is on a different platform");
        //     platform = newPlat;
        //     ShrinkPlayer(0.1f);
        //     // hasShrunk = true; // Mark that the player has shrunk so dont die from repeated shrinking
        // } 
        // Debug.Log("Setting isGrounded to true");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground")) 
        {
            Debug.Log("Player left the ground");
            isGrounded = false;
            highPt = false;
        }
    }

    void ShrinkPlayer(float sizeDecrease)
    {
        Debug.Log("Decreasing body mass by 0.1");
        
        // Ensure the player doesn't shrink below zero
        if (transform.localScale.x - sizeDecrease <= 0 || transform.localScale.y - sizeDecrease <= 0)
        {
            transform.localScale = Vector3.zero;  // Player is fully shrunk
        }
        else
        {
            transform.localScale -= new Vector3(sizeDecrease, sizeDecrease, 0);  // Shrink
        }
        
        moveSpeed += sizeDecrease;
    }
    void GrowPlayer(float sizeIncrease)
    {
        playerAnims.Play("GainMass");
        Debug.Log("size increase by: " + sizeIncrease);
        transform.localScale += new Vector3(sizeIncrease, sizeIncrease, 0);
        moveSpeed -= sizeIncrease;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;            

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50f;      
    public float fallThreshold = -10f;  
    public GameObject restartButton;   
    public GameObject inGameRestartButton;
    public LayerMask groundLayer;     

    private Rigidbody2D rb;           
    private Vector3 initialScale;     
    private bool isGrounded;

    public bool inverse = false;
    public float size = 5f;
    public string resizeDirection = "";
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
            if (moveMode == MoveMode.idle)
            {
                resizeDirection = "y";
                inverse = false;
                moveMode = MoveMode.stretch;
            }
        }
        /*else if (Input.GetAxis("Vertical") == -1)
        {
            resizeDirection = "y";
            inverse = true;
            moveMode = MoveMode.stretch;
        }*/

        // idle
        if (Input.GetAxis("Horizontal") == 0 && (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Vertical") == -1))
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
            Shrink(resizeAmount * 2, resizeDirection);
        }

        // player dies
        if (transform.position.y < fallThreshold)
        {
            PlayerDies();
        }
    }

    // Source: https://pastebin.com/4VsCvrs7 
    void Stretch(float amount, string direction)
    {
        if (stretchedAmount < size)
        {
            if (direction == "x" && inverse == false)
            {
                Debug.Log("HELLO");
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

            /*else if (direction == "y" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - (amount / 2));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + amount);
            }*/
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

    void PlayerDies()
    {

        restartButton.SetActive(true);


        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 100f;
    public float fallThreshold = -10f;
    public GameObject restartButton;
    public GameObject inGameRestartButton;
    // public LayerMask groundLayer;
    public Animator playerAnims;

    private Rigidbody2D rb;
    private Rigidbody2D objectRigidbody;
    private Vector3 initialScale;
    public bool isGrounded = true;
    // public bool hasShrunk = false; // flag to prevent repeated shrinking

    public string platform = "Gound";
    public string newPlat = "Ground";
    public bool highPt = false;
    public float hight = 0f;
    public float baseHight = 0f;

    public bool inverse = false;
    public float stretchSize = 1.2f;
    public string resizeDirection = "";
    public float speedMult = 5f;
    private float stretchedAmount = 0f;
    public float size = 1f;
    private bool dead = false;
    private bool jumped = false;

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
        hight = transform.position.y;
        baseHight = transform.position.y;
    }

    void FixedUpdate()
    {
        float resizeAmount = moveSpeed * Time.deltaTime;

        // horizontal movement
        if (Input.GetAxis("Horizontal") == 1)
        {
            if (moveMode == MoveMode.idle)
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
        if (Input.GetAxis("Vertical") == 1)
        {
            if (moveMode == MoveMode.idle && (isGrounded || !jumped))
            {
                resizeDirection = "y";
                inverse = false;
                moveMode = MoveMode.stretch;
                jumped = true;
            }
        }
        else if (Input.GetAxis("Vertical") == -1)
        {
            if (!isGrounded)
            {
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
        if (!dead && transform.position.y < fallThreshold)
        {
            PlayerDies();
        }

        if (rb.velocity.y < 0 && !highPt)
        {
            highPt = true;
            hight = transform.position.y;
            Debug.Log("highest height: " + hight);
        }
    }

    // Source: https://pastebin.com/4VsCvrs7 
    void Stretch(float amount, string direction)
    {
        //Debug.Log("STRETCH: " + direction);
        if (stretchedAmount < stretchSize)
        {
            if (direction == "x" && inverse == false)
            {
                transform.position = new Vector2(transform.position.x + (amount / 2), transform.position.y);
                transform.localScale = new Vector2(transform.localScale.x + amount, transform.localScale.y);
                /*if (transform.localScale.y > 0.2f)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - (amount / 2));
                    transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - amount);
                }*/
            }

            if (direction == "y" && inverse == false)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (amount / 2));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + amount);
                /*if (transform.localScale.x > 0.2f)
                {
                    transform.position = new Vector2(transform.position.x - (amount / 2), transform.position.y);
                    transform.localScale = new Vector2(transform.localScale.x - amount, transform.localScale.y);
                }*/                  
            }

            if (direction == "x" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x - (amount / 2), transform.position.y);
                transform.localScale = new Vector2(transform.localScale.x + amount, transform.localScale.y);
                /*if (transform.localScale.y > 0.2f)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - (amount / 2));
                    transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - amount);
                }*/
            }

            if (direction == "y" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - (amount / 2));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + amount);
                /*if (transform.localScale.x > 0.2f)
                {
                    transform.position = new Vector2(transform.position.x + (amount / 2), transform.position.y);
                    transform.localScale = new Vector2(transform.localScale.x - amount, transform.localScale.y);
                }*/
            }

            stretchedAmount += amount;
        }
        else // stretch limit
        {
            moveMode = MoveMode.shrink;
            if (direction == "y")
            {
                transform.localScale = new Vector2(size, size + stretchSize);
            }
            else if (direction == "x")
            {
                transform.localScale = new Vector2(size + stretchSize, size);
            }
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
                if(transform.localScale.y > size)
                {
                    //transform.position = new Vector2(transform.position.x, transform.position.y - (amount / 2));
                    transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - amount);
                }
            }
            if (direction == "y" && inverse == false)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + (amount));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - amount);
                if (transform.localScale.x > size)
                {
                    //transform.position = new Vector2(transform.position.x - (amount / 2), transform.position.y);
                    transform.localScale = new Vector2(transform.localScale.x - amount, transform.localScale.y);
                }
            }

            if (direction == "x" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x - (amount / 2), transform.position.y);
                transform.localScale = new Vector2(transform.localScale.x - amount, transform.localScale.y);
                if (transform.localScale.y > size)
                {
                    //transform.position = new Vector2(transform.position.x, transform.position.y + (amount / 2));
                    transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - amount);
                }
            }
            if (direction == "y" && inverse == true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - (amount / 2));
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y - amount);
                if (transform.localScale.x > size)
                {
                    //transform.position = new Vector2(transform.position.x + (amount / 2), transform.position.y);
                    transform.localScale = new Vector2(transform.localScale.x - amount, transform.localScale.y);
                }
            }
            stretchedAmount -= amount;
        }
        else // no more shrinking
        {
            rb.gravityScale = 1;
            moveMode = MoveMode.idle;
            transform.localScale = new Vector2(size, size);
        }
    }

    public void PlayerDies()
    {
        Debug.Log("PlayerDies called");
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        playerAnims.SetTrigger("Dead");
        dead = true;
    }

    public void PlayerDestroyed()
    {
        restartButton.SetActive(true);
        Destroy(gameObject);
        Debug.Log("PlayerDestroyed called");
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
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            //check the name of the platform
            platform = collision.gameObject.name;
            Debug.Log("Player has landed on " + platform);
            if (platform == "BreakingPlatform")
            {
                if (size > 5)
                {
                    Destroy(collision.gameObject);
                }
            }
            isGrounded = true;
            jumped = false;
            baseHight = transform.position.y;
            Debug.Log("starting height: " + baseHight);
            Debug.Log("height difference: " + Math.Abs(hight - baseHight));
            Debug.Log("take damge if its greater than: " + (size * 1.5f));
            if (Math.Abs(hight - baseHight) > (size * 1.5f) && !(inverse == true && resizeDirection == "y"))
            {
                ShrinkPlayer(Math.Abs(hight - baseHight) * 0.5f);
            }
        } else if (collision.collider.gameObject.CompareTag("Object"))
        {
            objectRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if(size > 5){
                Debug.Log("Player pushing with object");
                // player is able to push the object
                objectRigidbody.constraints = RigidbodyConstraints2D.None;
            } else {
                Debug.Log("Player is too small to push object");
                // the object stays in place
                objectRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            // Debug.Log("Player left the ground");
            isGrounded = false;
            highPt = false;
        }
    }

    void ShrinkPlayer(float sizeDecrease)
    {
        // Debug.Log("Decreasing body mass by " + sizeDecrease);
        // Ensure the player doesn't shrink below zero
        if (transform.localScale.x - sizeDecrease <= 0 || transform.localScale.y - sizeDecrease <= 0)
        {
            Debug.Log("Died from shrinkage localscale");
            PlayerDies();
        }
        else
        {
            moveSpeed += sizeDecrease;
            size -= sizeDecrease;
            if (size <= 0)
            {
                Debug.Log("Died from shrinkage");
                PlayerDies();
            }
            else
            {
                playerAnims.SetTrigger("LoseMass");
                transform.localScale -= new Vector3(sizeDecrease, sizeDecrease, 0);  // Shrink
            }
        }
    }
    void GrowPlayer(float sizeIncrease)
    {
        playerAnims.SetTrigger("GainMass");
        Debug.Log("size increase by: " + sizeIncrease);
        transform.localScale += new Vector3(sizeIncrease, sizeIncrease, 0);
        Debug.Log("LOCAL SCALE: " + transform.localScale);
        moveSpeed -= sizeIncrease;
        size += sizeIncrease;
        stretchSize += sizeIncrease*2;
    }
}

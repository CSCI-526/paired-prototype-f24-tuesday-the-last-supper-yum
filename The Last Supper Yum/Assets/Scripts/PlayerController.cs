using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;            

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;      
    public float fallThreshold = -10f;  
    public GameObject restartButton;   
    public GameObject inGameRestartButton;
    public LayerMask groundLayer;     

    private Rigidbody2D rb;           
    private Vector3 initialScale;     
    private bool isGrounded;

    public bool inverse = false;
    public float resizeAmount = 5f;
    public string resizeDirection = "";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        initialScale = transform.localScale;

        restartButton.SetActive(false);
    }

    void FixedUpdate()
    {
       
        float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(move, 0, 0);

        if (transform.position.y < fallThreshold)
        {
            PlayerDies();
        }
    }

    /*void FixedUpdate()
    {
        // horizontal movement
        if (Input.GetAxis("Horizontal") == 1)
        {
            
        }
        else if (Input.GetAxis("Horizontal") == -1)
        {
        }

        // vertical movement
        if (Input.GetAxis("Vertical") == 1) 
        { 
        }
        else if (Input.GetAxis("Vertical") == -1)
        {

        }

        // idle
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {

        }

        resize(resizeAmount, resizeDirection);

        // player dies
        if (transform.position.y < fallThreshold)
        {
            PlayerDies();
        }
    }*/

    void resize(float amount, string direction)
    {
        if (direction == "x" && inverse == false)
        {
            transform.position = new Vector3(transform.position.x + (amount / 2), transform.position.y, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x + amount, transform.localScale.y, transform.localScale.z);
        }
        else if (direction == "y" && inverse == false)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (amount / 2), transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + amount, transform.localScale.z);
        }

        if (direction == "x" && inverse == true)
        {
            transform.position = new Vector3(transform.position.x - (amount / 2), transform.position.y, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x + amount, transform.localScale.y, transform.localScale.z);
        }
        else if (direction == "y" && inverse == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (amount / 2), transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + amount, transform.localScale.z);
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

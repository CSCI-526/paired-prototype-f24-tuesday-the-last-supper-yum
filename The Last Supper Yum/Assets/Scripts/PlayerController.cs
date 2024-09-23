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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        initialScale = transform.localScale;

        restartButton.SetActive(false);
    }

    void Update()
    {
       
        float move = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        transform.Translate(move, 0, 0);

        if (transform.position.y < fallThreshold)
        {
            PlayerDies();
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

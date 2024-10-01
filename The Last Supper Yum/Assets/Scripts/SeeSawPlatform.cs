using UnityEngine;

public class SeeSawPlatform : MonoBehaviour
{
    public float Weight = 1f;  
    public float maxRotationAngle = 30f;
    public float smoothness = 2f;    

    private HingeJoint2D hingeJoint;
    private bool playerOnPlatform = false;
    private Transform player;
    private PlayerController playerController;

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = collision.collider.transform;
            playerController = player.GetComponent<PlayerController>();
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    void FixedUpdate()
    {
        if (playerOnPlatform && player != null)
        {
            float playerOffset = player.position.x - transform.position.x;
            float targetRotation = Mathf.Clamp(playerOffset * playerController.size * Weight, -maxRotationAngle, maxRotationAngle);
            float newRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetRotation, Time.fixedDeltaTime * smoothness);
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
        else
        {
            float newRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, 0, Time.fixedDeltaTime * (smoothness / 2));
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
    }
}

using UnityEngine;

public class SeeSawPlatform : MonoBehaviour
{
    public float weightFactor = 1f;    // Factor to determine how much weight affects rotation
    public float maxRotationAngle = 30f;  // Maximum angle the platform can tilt
    public float smoothness = 2f;     // How smoothly the platform tilts

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
            // Calculate the player's horizontal offset from the center of the platform
            float playerOffset = player.position.x - transform.position.x;

            // Calculate how much the platform should rotate based on the player's position and weight
            float targetRotation = Mathf.Clamp(playerOffset * playerController.size * weightFactor, -maxRotationAngle, maxRotationAngle);

            // Smoothly rotate the platform
            float newRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetRotation, Time.fixedDeltaTime * smoothness);
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
        else
        {
            // Reset the platform back to its neutral position when no player is on it
            float newRotation = Mathf.LerpAngle(transform.rotation.eulerAngles.z, 0, Time.fixedDeltaTime * (smoothness / 2));
            transform.rotation = Quaternion.Euler(0, 0, newRotation);
        }
    }
}

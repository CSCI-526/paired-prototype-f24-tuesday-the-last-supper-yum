using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSawPlatform : MonoBehaviour
{
    public float rotationSpeed = 3f;    
    public float maxRotationAngle = 30f;  
    public float weightFactor = 1f;   
    public float edgeDampingFactor = 0.7f;

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

    void Update()
    {
        if (playerOnPlatform)
        {
            float playerPositionX = player.position.x - transform.position.x;
            
            float distanceFromCenter = Mathf.Abs(playerPositionX);
            
            float rotationAmount = -playerPositionX * playerController.size * weightFactor;

            float damping = 1f - Mathf.Clamp01(distanceFromCenter / maxRotationAngle) * edgeDampingFactor;
            rotationAmount *= damping;

   
            rotationAmount = Mathf.Clamp(rotationAmount, -maxRotationAngle, maxRotationAngle);

           
            hingeJoint.transform.rotation = Quaternion.Lerp(
                hingeJoint.transform.rotation,
                Quaternion.Euler(0, 0, rotationAmount),
                Time.deltaTime * rotationSpeed
            );
        }
        else
        {
            hingeJoint.transform.rotation = Quaternion.Lerp(
                hingeJoint.transform.rotation,
                Quaternion.identity, 
                Time.deltaTime * (rotationSpeed / 2)
            );
        }
    }
}

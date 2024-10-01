using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public float platSize = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.collider.gameObject.CompareTag("Player"))
        {
            float playerSize = collision.gameObject.GetComponent<PlayerController>().size;
            if(playerSize > platSize){
                Destroy(gameObject);
            }
        } 
    }
}

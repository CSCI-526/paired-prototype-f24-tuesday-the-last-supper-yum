using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    public GameObject key;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == key)
        {
            Destroy(gameObject);
        }
    }
}

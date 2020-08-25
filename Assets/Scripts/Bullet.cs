using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody.velocity = transform.up * speed;        
    }

    void OnCollisionEnter2D(Collision2D hitInfo)
    {
        // No matter what the bullet hits, it will disappear
        Destroy(gameObject);
    }

}

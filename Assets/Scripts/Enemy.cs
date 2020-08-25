using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerTransform;
    Rigidbody2D m_RigidBody2D;
    bool isFreeze = false;
    bool facingRight = true;
    private Animator m_animator;
    // Start is called before the first frame update
    void Awake()
    {
        m_RigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        m_animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFreeze) return;
        // Cast a ray to the player position
        var rayCastDirection = playerTransform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayCastDirection, Mathf.Infinity);

        if (hit.collider.gameObject.tag == "Player")
        {


            // Don't chase the player when they're out of sight
            //if player in front of the enemies
            if (playerTransform.position.x < transform.position.x)
            {

                this.transform.position += new Vector3(-5.0f * Time.deltaTime, 0f, 0f);
            }
            //if player is behind enemies
            else if (playerTransform.position.x > transform.position.x)
            {
                this.transform.position += new Vector3(5.0f * Time.deltaTime, 0f, 0f);
            }
            m_animator.SetFloat("Speed", 1f);
        }
        else
        {
            m_animator.SetFloat("Speed", 0f);
        }
        var velocity = m_RigidBody2D.velocity;
        if (velocity.x < 0 && facingRight)
        {
            Flip();
        }
        else if (velocity.x > 0 && !facingRight)
        {
            Flip();
        }


    }

    void FixedUpdate()
    {
        var velocity = m_RigidBody2D.velocity;
        velocity.y = 0;
        m_animator.SetFloat("Speed", velocity.magnitude);
    }

    private void Flip()
    {
        Debug.Log("Flip");
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // Destroy object if hit by a bullet
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            // End game
        }
    }

    public void setPlayer(GameObject player)
    {
        playerTransform = player.GetComponent<Transform>();
    }
    public void setFreeze(bool isFreeze)
    {
        this.isFreeze = isFreeze;
        if (!isFreeze)
            m_RigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        else
            m_RigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}

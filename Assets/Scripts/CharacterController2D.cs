using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class CharacterController2D : MonoBehaviour
{

    [SerializeField] private Transform m_GroundCheck;
    const float k_GroundedRadius = .2f;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private float m_JumpForce = 16.0f;
    public float m_speed = 10.0f;
    private bool grounded = false;
    private float m_MovementSmoothing = 0.01f;	// How much to smooth out the movement

    private float m_StartActionTime;
    private float m_PlayTime;
    private int m_CallCount = 0;
    public TimelineManager m_TimelineManager;

    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    public Vector3 m_Velocity = Vector3.zero;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private GameController gameController;

    bool isFreeze = true;

    private int m_BulletRemaining = 3;

    private Rigidbody2D m_RigidBody2D;
    private Animator m_animator;

    public void Move(float movement)
    {
        Vector3 targetVelocity = new Vector2(movement * m_speed, m_RigidBody2D.velocity.y);
        m_RigidBody2D.velocity = Vector3.SmoothDamp(m_RigidBody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if ((movement > 0 && !m_FacingRight) || (movement < 0 && m_FacingRight))
            Flip();
    }


    public bool Jump()
    {
        GroundCheck();
        if (!grounded)
            return false;
        grounded = false;
        // Cancel out y velocity for consistent jump height
        SoundManager.PlaySound("Jump");
        m_RigidBody2D.velocity = new Vector2(m_RigidBody2D.velocity.x, 0.0f);
        m_RigidBody2D.AddForce(m_JumpForce * transform.up, ForceMode2D.Impulse);
        return true;
    }

    private float GetMoveDirection()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            return -1.0f;
        }
        else
        {
            return 1.0f;
        }
    }

    void Awake()
    {
        m_RigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        m_animator = gameObject.GetComponent<Animator>();
    }

    void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				grounded = true;
			}
		}
    }

    void Update()
    {
        if (!isFreeze) return;
        if (!m_TimelineManager.m_MoveTimeline.Locked)
        {
            if (Input.GetButtonDown("Horizontal"))
            {
                // Start recording horizontal movement
                m_StartActionTime = Time.time;
                float direction = GetMoveDirection();
                float currentTime = Time.time;
                float duration = currentTime - m_StartActionTime;
                Action action = new MoveAction(m_StartActionTime - m_PlayTime, duration, direction, 1);
                m_TimelineManager.RecordAction(action);
                Move(direction);
            }


            if (Input.GetButton("Horizontal"))
            {
                float direction = GetMoveDirection();
                float currentTime = Time.time;
                float duration = currentTime - m_StartActionTime;
                m_TimelineManager.m_MoveTimeline.UpdateDurationLastAction(duration);
                m_TimelineManager.m_MoveTimeline.UpdateDirectionLastAction(direction);
                Move(direction);
            }

            if (Input.GetButtonUp("Horizontal"))
            {
                float direction = GetMoveDirection();
                float currentTime = Time.time;
                float duration = currentTime - m_StartActionTime;
                m_TimelineManager.m_MoveTimeline.UpdateDurationLastAction(duration);
                Debug.Log("Add new action at " + m_StartActionTime + ". Move timeline: " + m_TimelineManager.m_MoveTimeline.Length());
            }
        }



        // Jump
        if (Input.GetButtonDown("Jump") && !m_TimelineManager.m_JumpTimeline.Locked)
        {
            float time = Time.time - m_PlayTime;
            Action action = new JumpAction(time);
            if (Jump()) m_TimelineManager.RecordAction(action);
            Debug.Log("Jump timeline: " + m_TimelineManager.m_JumpTimeline.Length());
        }


        // Shoot
        if (Input.GetButtonDown("Fire1") && m_BulletRemaining > 0 && !m_TimelineManager.m_ShootTimeline.Locked)
        {
            float time = Time.time - m_PlayTime;
            Action action = new ShootAction(time);
            Fire();
            m_TimelineManager.RecordAction(action);
            Debug.Log("Shoot timeline: " + m_TimelineManager.m_ShootTimeline.Length());
        }



    }

    void FixedUpdate()
    {
        var velocity = m_RigidBody2D.velocity;
        velocity.y = 0;
        m_animator.SetFloat("Speed", velocity.magnitude);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        grounded = false;
    }


    public void Fire()
    {
        if (m_BulletRemaining >= 0)
        {
            SoundManager.PlaySound("Attack");
            m_animator.SetTrigger("isAttack");
            Debug.Log("Fire");
            Quaternion rotation = firePoint.rotation;
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.Rotate(0, 0, -90);
            bullet.tag = "Bullet";
            m_BulletRemaining -= 1;

        }
    }


    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Goal")
        {
            Debug.Log("Finshed Round!!!!!!!!!");
            gameController.FinishLevel();
        }
        else if (collisionInfo.gameObject.tag == "Enemy")
        {
            gameController.GameOver();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    public void setLocked(bool isMovingLock, bool isJumpingLock, bool isAttackLock)
    {
        m_TimelineManager.m_JumpTimeline.Locked = isJumpingLock;
        m_TimelineManager.m_MoveTimeline.Locked = isMovingLock;
        m_TimelineManager.m_ShootTimeline.Locked = isAttackLock;
    }

    public void addController(GameController gameController)
    {
        this.gameController = gameController;
    }
    
    public void Replay()
    {
        if (m_TimelineManager.m_JumpTimeline.Locked) m_TimelineManager.m_JumpTimeline.Replay();
        if (m_TimelineManager.m_MoveTimeline.Locked) m_TimelineManager.m_MoveTimeline.Replay();
        if (m_TimelineManager.m_ShootTimeline.Locked) m_TimelineManager.m_ShootTimeline.Replay();

    }
    public void setFreeze(bool isFreeze)
    {
        this.isFreeze = isFreeze;        
        if (!isFreeze)
        {
            m_animator.enabled = false;
            m_RigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            m_PlayTime = Time.time;
            m_animator.enabled = true;
            m_RigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}

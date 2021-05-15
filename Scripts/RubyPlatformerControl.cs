using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubyPlatformerControl : MonoBehaviour
{
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public GameObject projectilePrefab;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public float speed = 6.0f;
    public float jumpPower = 20f;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    Rigidbody2D rigidbody2d;
    float horizontal;
    //float vertical;

    bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;
    bool doubleJump = false;



    public ParticleSystem DamageEffect;
    private ParticleSystem DamageCollisionEffect;


    public AudioSource audioSource;
    public AudioClip throwingClip;
    public AudioClip hitClip;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        //         audioSource = GetComponent<AudioSource>();
        //         walkingAudioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");
        //Debug.Log(vertical);
        Vector2 move = new Vector2(horizontal * speed, rigidbody2d.velocity.y);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {

            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }


        animator.SetFloat("Look X", lookDirection.x);
        //animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);




        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                doubleJump = true;
            }
            else if (doubleJump)
            {

                Jump();
                doubleJump = false;

            }
        }

        //         if (Input.GetKeyDown(KeyCode.C))
        //         {
        //             Launch();
        //             PlaySound(throwingClip);
        //         }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    private void FixedUpdate()
    {

        Vector2 movement = new Vector2(horizontal * speed, rigidbody2d.velocity.y);
        rigidbody2d.velocity = movement;
        //         Vector2 position = rigidbody2d.position;
        //         position.x = position.x + speed * horizontal * Time.deltaTime;
        // 
        //         //position.y = position.y + jumpPower * 1 * Time.deltaTime;
        // 
        //         Debug.Log(position.y);
        // 
        // 
        //         rigidbody2d.MovePosition(position);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    void Jump()
    {
        Vector2 movement = new Vector2(rigidbody2d.velocity.x, jumpPower);
        rigidbody2d.velocity = movement;
    }


    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            if (isInvincible == false)
            {
                TakeDamage();
                PlaySound(hitClip);
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        if (currentHealth == 0)
        {
            SceneManager.LoadScene(2);
        }
    }



    public void TakeDamage()
    {
        DamageCollisionEffect = Instantiate(DamageEffect, transform.position, Quaternion.identity) as ParticleSystem;
        DamageCollisionEffect.Play();
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}

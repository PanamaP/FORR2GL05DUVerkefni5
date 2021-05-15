using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public GameObject projectilePrefab;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public float speed = 3.0f;


    public int countRobots;
    int count = 0;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public ParticleSystem DamageEffect;
    private ParticleSystem DamageCollisionEffect;


    public AudioSource audioSource;
    public AudioClip throwingClip;
    public AudioClip hitClip;
    public AudioSource walkingAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        // telur fjolda velmenna inna senu

        countRobots = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log(countRobots);

        //         audioSource = GetComponent<AudioSource>();
        //         walkingAudioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {

            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        // animation
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        // spilar hljod ef leikmadur er ad hlaupa
        if (move.magnitude >= 0.1 && !walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
        }
        else if (move.magnitude <= 0.1 && walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();
        }


        // leikmadur getur ekki tekið damage i akveðin tima
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
            PlaySound(throwingClip);
        }

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
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

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
            SceneManager.LoadScene(3);
        }
    }

    public void Counter()
    {
        count = count + 1;
        Debug.Log(count);
        if (countRobots <= count)
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

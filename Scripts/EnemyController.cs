using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{

    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;
    float timer;
    int direction = 1;
    bool broken;

    AudioSource audioSource;
    public AudioClip robotFixClip;

    Animator animator;

    Rigidbody2D rigidbody2d;
    public RubyController rubyCount;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        broken = true;




        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (!broken)
        {
            return;
        }

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }


        rigidbody2d.MovePosition(position);
    }

    public void Fix()
    {

        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(robotFixClip);
        rubyCount.Counter();

    }


    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
}

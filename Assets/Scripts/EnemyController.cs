using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public ParticleSystem smokeEffect;

    new Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool broken = true;
    public bool hard;
    SpriteRenderer sprite;

    public RubyController controller;

    public AudioSource audioSource;
    public AudioClip fixedSound;


    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        if (hard)
        {
            sprite.material.SetColor("_Color", Color.red);
            speed = 5;
        }
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

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

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null && !hard && !player.dead)
        {
            player.ChangeHealth(-1);
        }
        if (player != null && hard && !player.dead)
        {
            player.ChangeHealth(-2);
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        audioSource.loop = false;
        audioSource.volume = 1.0f;
        audioSource.PlayOneShot(fixedSound);
        broken = false;
        rigidbody2D.simulated = false;
        sprite.material.SetColor("_Color", Color.white);
        controller.robotsToFix -= 1;
        controller.scoreText.text = "Robots to Fix: " + controller.robotsToFix.ToString();
        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");

        smokeEffect.Stop();
    }
}
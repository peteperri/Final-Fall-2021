using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpidieController : MonoBehaviour
{
    public int speed;
    public int maxDistance;
    public int minDistance;
    private Rigidbody2D rb;
    private RubyController player;
    public int health;
    public float flashTime;
    private Color originalColor;
    public SpriteRenderer renderer;
    public AudioClip crunchClip;
    private Animator anim;
    private BoxCollider2D _collider2D;

    // Start is called before the first frame update
    void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        originalColor = renderer.color;
        rb = GetComponent<Rigidbody2D>();
        player = (RubyController)FindObjectOfType(typeof(RubyController));
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.spidieKilled)
        {
            transform.LookAt(player.transform);
            if (Vector3.Distance(transform.position, player.transform.position) >= minDistance)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }

            if (player.transform.position.x - transform.position.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        if (player.spidieKilled)
        {
            _collider2D.enabled = false;
            anim.SetBool("dead", true);
        }

    }
    protected void LateUpdate()
    {
        if (!player.spidieKilled)
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<RubyController>() != null && !player.spidieKilled)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Damage()
    {
        health -= 1;
        FlashRed();
        if (health <= 0)
        {
            player.spidieKilled = true;
        }
    }
    
    void FlashRed()
    {
        AudioSource.PlayClipAtPoint(crunchClip, transform.position, 1);
        renderer.color = Color.red;
        Invoke("ResetColor", flashTime);
    }
    void ResetColor()
    {
        renderer.color = originalColor;
    }
}

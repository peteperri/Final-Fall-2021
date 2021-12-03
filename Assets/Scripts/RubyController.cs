using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool dead = false;
    public int maxHealth = 5;

    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;
    public AudioSource bgMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    public ParticleSystem healthEffect;
    public ParticleSystem damageEffect;

    public int robotsToFix = 4;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI ammoText;
    bool winAudioNotPlayed = true;

    private static int level = 1;

    bool canShoot = true;

    bool gameWon = false;

    public bool unlimitedAmmo = false;

    public int ammo = 4;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }


        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && canShoot)
        {
            Launch();
            canShoot = false;
            StartCoroutine(WaitTilNextShot(0.5f));
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

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (robotsToFix == 0 && level == 1)
        {
            if (winAudioNotPlayed == true)
            {
                bgMusic.Stop();
                audioSource.loop = false;
                audioSource.PlayOneShot(winMusic);
                winAudioNotPlayed = false;
            }

            scoreText.text = "Talk to Jambi to go to the next level.";
        }

        if (robotsToFix == 0 && level == 2)
        {
            gameWon = true;
        }

        if (gameWon)
        {
            if (winAudioNotPlayed == true)
            {
                audioSource.volume = 0.5f;
                bgMusic.Stop();
                audioSource.loop = false;
                audioSource.PlayOneShot(winMusic);
                winAudioNotPlayed = false;
            }
            winText.text = "You win! Game by Peter Perri. Press R to restart.";
            if (Input.GetKey(KeyCode.R))
            {
                level = 1;
                gameWon = false;
                SceneManager.LoadScene("Main");
            }
        }

        if (health == 0)
        {
            dead = true;
            animator.SetTrigger("Dead");
            audioSource.volume = 0.03f;
            bgMusic.Stop();
            audioSource.loop = false;
            audioSource.PlayOneShot(loseMusic);
            loseText.text = "You Lose! Press R To Restart.";
            if (Input.GetKey(KeyCode.R) && level == 1)
            {
                SceneManager.LoadScene("Main");
            }
            if (Input.GetKey(KeyCode.R) && level == 2)
            {
                SceneManager.LoadScene("Level2");
            }
        }
    }

    void FixedUpdate()
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

            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlaySound(hitSound);
            damageEffect = Instantiate(damageEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            damageEffect.Play();
        }
        else if (amount > 0)
        {
            healthEffect = Instantiate(healthEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            healthEffect.Play();
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        if (ammo > 0 || unlimitedAmmo)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 500);

            animator.SetTrigger("Launch");

            PlaySound(throwSound);
            if(!unlimitedAmmo)
            {
                ammo--;
                ammoText.text = "Gears: " + ammo.ToString();
            }
            else{
                ammoText.text = "Gears: ∞";
            }
            

        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public int getLevel()
    {
        return level;
    }

    public void setLevel(int passedLevel)
    {
        level = passedLevel;
    }

    IEnumerator WaitTilNextShot(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

}

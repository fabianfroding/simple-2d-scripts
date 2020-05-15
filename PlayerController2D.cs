using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: If player falls from platform (without pressing space) he never "lands"

public class PlayerController2D : MonoBehaviour
{
    int health = 5;
    private Material matWhite;
    private Material matDefault;
    bool invulnerable = false;

    public List<string> items;

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    bool isGrounded;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Transform groundCheckL;

    [SerializeField]
    Transform groundCheckR;

    [SerializeField]
    private float runSpeed = 1.5f;

    [SerializeField]
    private float jumpSpeed = 5f;

    Object bulletRef;

    public AudioSource collectOrb;

    void Start()
    {
        items = new List<string>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletRef = Resources.Load("Bullet");
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.Play("Player_shoot");
            GameObject bullet = (GameObject)Instantiate(bulletRef);
            bullet.GetComponent<BulletScript>().playBirthSound();
            float bulletX = .4f;

            if (spriteRenderer.flipX)
            {
                bulletX = -.4f;
                bullet.GetComponent<SpriteRenderer>().flipX = true;
            }
            bullet.transform.position = new Vector3(transform.position.x + bulletX, transform.position.y + .2f, -1);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            animator.Play("Player_shoot");
            for (int i = 0; i < 4; i++)
            {
                GameObject bullet = (GameObject)Instantiate(bulletRef);
                bullet.GetComponent<BulletScript>().source = this.gameObject;
                if (i == 0)
                {
                    bullet.GetComponent<BulletScript>().playBirthSound();
                }
                else
                {
                    bullet.GetComponent<BulletScript>().playDeathSound = false;
                }
                bullet.GetComponent<BulletScript>().spiral = true;
                bullet.GetComponent<BulletScript>()._angle = 77f * i;
                bullet.GetComponent<BulletScript>().timedLife = 5.0f;
                bullet.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

                if (spriteRenderer.flipX)
                {
                    bullet.GetComponent<SpriteRenderer>().flipX = true;
                }

                bullet.transform.position = new Vector3(transform.position.x, transform.position.y + .2f, -1);
            }
            

        }
    }

    void FixedUpdate()
    {
        
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            animator.Play("Player_jump");
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);

            if (isGrounded)
            {
                animator.Play("Player_run");
            }
            
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);

            if (isGrounded)
            {
                animator.Play("Player_run");
            }

            spriteRenderer.flipX = true;
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            if (isGrounded)
            {
                animator.Play("Player_idle");
            }
        }

        if (Input.GetKey("space") && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            animator.Play("Player_jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Collectable"))
        {
            string itemType = collider.gameObject.GetComponent<CollectableScript>().itemType;
            if (itemType == "Orb")
            {
                collectOrb.Play();
            }
            items.Add(itemType);
            Debug.Log("Items: " + items.Count);
            Destroy(collider.gameObject);
        }

        if ((collider.CompareTag("DarkOrb") || collider.CompareTag("ShadowCoil")) && !invulnerable)
        {
            invulnerable = true;
            health--;
            spriteRenderer.material = matWhite;
            if (health <= 0)
            {
                // Kill player
                KillSelf();
            }
            else
            {
                Invoke("ResetMaterial", .1f);
                Invoke("MakeVulnerable", .5f);
            }
            if (collider.CompareTag("ShadowCoil"))
            {
                collider.GetComponent<ShadowCoil>().DestroySelf();
            }
        }

    }

    void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    void MakeVulnerable()
    {
        invulnerable = false;
    }

    private void KillSelf()
    {
        gameObject.SetActive(false);
    }
}

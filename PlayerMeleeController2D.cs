using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeController2D : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    GameObject attackHitbox;

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

    bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            if (isGrounded)
            {
                int index = UnityEngine.Random.Range(0, 4);
                animator.Play("PlayerMelee_attack" + index);
            }
            else
            {
                animator.Play("PlayerMelee_flykick");
            }
            StartCoroutine(DoAttack());
        }
        
    }

    IEnumerator DoAttack()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(.4f);
        attackHitbox.SetActive(false);
        isAttacking = false;
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
            if (!isAttacking)
            {
                animator.Play("PlayerMelee_jump");
            }
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);

            if (isGrounded && !isAttacking)
            {
                animator.Play("PlayerMelee_run");
            }

            //spriteRenderer.flipX = false;
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);

            if (isGrounded && !isAttacking)
            {
                animator.Play("PlayerMelee_run");
            }

            //spriteRenderer.flipX = true;
            transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            if (isGrounded && !isAttacking)
            {
                animator.Play("PlayerMelee_idle");
            }
        }

        if (Input.GetKey("space") && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            animator.Play("PlayerMelee_jump");
        }
    }

}

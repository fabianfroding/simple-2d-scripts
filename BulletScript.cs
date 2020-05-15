using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    public AudioSource bulletLaunch;
    public AudioSource bulletHit;

    public float direction = 0;

    public bool spiral = false;

    //===============
    private float RotateSpeed = 4f;
    private float Radius = 1.5f;

    public Vector2 _centre;
    public float _angle;
    //===============

    public bool playDeathSound = true;

    public float timedLife = .7f;

    public GameObject source;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("DestroySelf", timedLife); // Not efficient way to delay.
    }

    public void playBirthSound()
    {
        bulletLaunch.Play();
    }

    private void Update()
    {
        if (spiral)
        {
            _centre = source.transform.position;
            _angle += RotateSpeed * Time.deltaTime;

            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = _centre + offset;
        }
    }

    void FixedUpdate()
    {
        float moveSpeed = 3;
        if (spriteRenderer.flipX)
        {
            moveSpeed = -3;
        }
        rb2d.velocity = new Vector2(moveSpeed, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        CancelInvoke();
        spriteRenderer.enabled = false;
        rb2d.velocity = new Vector2(0, 0);
        GetComponent<BoxCollider2D>().enabled = false;
        if (playDeathSound)
        {
            bulletHit.Play();
        }
        
        Destroy(gameObject, bulletHit.clip.length);
    }

}

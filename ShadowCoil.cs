using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCoil : MonoBehaviour
{
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;

    bool homing = true;

    public GameObject player;
    Transform target;


    public float angleChangingSpeed = 300f;
    public float movementSpeed = 1f;
    Vector2 direction;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player").transform;

        Vector3 targ = target.transform.position;
        targ.z = 0f;
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        Invoke("StopHoming", 2f);
        Invoke("DestroySelf", 6f);
    }

    void FixedUpdate()
    {
        if (homing)
        {
            direction = (Vector2)target.position - rb2d.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb2d.angularVelocity = -angleChangingSpeed * rotateAmount;
            rb2d.velocity = transform.up * movementSpeed;
        }
        
        
    }

    private void StopHoming()
    {
        homing = false;
        rb2d.angularVelocity = 0;
    }

    public void DestroySelf()
    {
        spriteRenderer.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;

        Destroy(gameObject);
    }
}

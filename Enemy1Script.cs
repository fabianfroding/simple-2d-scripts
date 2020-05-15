using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Script : MonoBehaviour
{
    private int health = 5;

    private Material matWhite;
    private Material matDefault;

    SpriteRenderer spriteRenderer;

    private UnityEngine.Object explosionRef;
    private UnityEngine.Object enemyRef;

    [SerializeField]
    private int delayBeforeDestroy = 5;

    Vector3 startPos;

    //===== Aggro stuff
    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb2d;

    [SerializeField]
    GameObject face;
    Animator faceAnimator;
    //=====

    //===== LoS
    [SerializeField]
    Transform castPoint;

    bool isFacingLeft = true;

    bool isAgro = false;
    bool isSearching;

    //=====

    void Start()
    {
        enemyRef = Resources.Load("Enemy1");
        spriteRenderer = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;
        explosionRef = Resources.Load("Explosion");
        startPos = transform.position;

        rb2d = GetComponent<Rigidbody2D>();
        //faceAnimator = face.GetComponent<Animator>();

        int initFacing = -1;
        isFacingLeft = false;
        System.Random rand = new System.Random();
        int temp = rand.Next(0, 2);
        if(temp == 1)
        {
            initFacing = 1;
            isFacingLeft = true;
        }
        transform.localScale = new Vector2(initFacing, 1); // Flips entire object, not just sprite graphic.

        TurnAround();
    }

    private void Update()
    {
        if (CanSeePlayer(agroRange))
        {
            isAgro = true;
        }
        else
        {
            if(isAgro)
            {
                if (!isSearching)
                {
                    isSearching = true;
                    Debug.Log("invoking stop");
                    Invoke("StopChasingPlayer", 3);
                }
                
            }
        }

        if (isAgro && GetDistanceToPlayer() > 0.52f)
        {
            ChasePlayer();
        }
    }

    float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    void TurnAround()
    {
        if (!isAgro)
        {
            System.Random rand = new System.Random();
            int temp = rand.Next(0, 2);
            if (temp == 1)
            {
                isFacingLeft = true;
                temp = 1;
            }
            else
            {
                isFacingLeft = false;
                temp = -1;
            }
            transform.localScale = new Vector2(temp, 1);
        }
        System.Random r = new System.Random();
        int t = r.Next(3, 6);
        Invoke("TurnAround", t);
    }


    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float castDist = distance;

        if(isFacingLeft)
        {
            castDist = -distance;
        }

        Vector2 endPos = castPoint.position + Vector3.right * castDist; // same as ... new Vector3(pos.x + dist)
        
        // Linecast is looking to make contact with anything on a layer called Action.
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if(hit.collider != null)
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                player = hit.collider.gameObject.transform;
                val = true;
            }
            else
            {
                val = false;
            }

            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }

        Debug.DrawLine(castPoint.position, endPos, Color.blue);

        return val;
    }
    

    private void ChasePlayer()
    {
        //TODO: add offset so it doesnt flip like crazy when standing above/below

        // Check if player is on right/left of enemy.
        if (transform.position.x < player.position.x)
        {
            rb2d.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1); // Flips entire object, not just sprite graphic.
            isFacingLeft = false;
        }
        else
        {
            rb2d.velocity = new Vector2(-moveSpeed, 0);
            transform.localScale = new Vector2(1, 1);
            isFacingLeft = true;
        }

        //faceAnimator.Play("face_eyes_open");
    }

    private void StopChasingPlayer()
    {
        Debug.Log("stopped chase player");
        isAgro = false;
        isSearching = false;
        rb2d.velocity = Vector2.zero;
        //faceAnimator.Play("face_eyes_closed");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Move this to bullet object instead, and look for enemy tag.
        if (collision.CompareTag("Bullet"))
        {
            collision.gameObject.GetComponent<BulletScript>().DestroySelf();
            health--;
            spriteRenderer.material = matWhite;
            if (health <= 0)
            {
                KillSelf();
            }
            else
            {
                Invoke("ResetMaterial", .1f);
            }
        }
    }

    void ResetMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    private void KillSelf()
    {
        GameObject explosion = (GameObject)Instantiate(explosionRef);
        explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);
        
        gameObject.SetActive(false);

        Invoke("Respawn", delayBeforeDestroy);
    }

    private void Respawn()
    {
        GameObject enemyClone = (GameObject)Instantiate(enemyRef);
        // Get old delayBeforeDestroy to "inherit it from previous enemy" or set it in resources.
        // If he spawns on another object like floor that is next to him he dissapears kind of.
        enemyClone.transform.position = new Vector3(UnityEngine.Random.Range(startPos.x - 3, startPos.x + 3), startPos.y, startPos.z);
        //enemyClone.GetComponent<Enemy1Script>().player = player;
        Destroy(gameObject);
    }

}
